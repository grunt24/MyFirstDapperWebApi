using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyFirstDapperProject.Repository.Interface;
using Dapper;

namespace MyFirstDapperProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        IDbConnection _connection;
        readonly string connectionString = "Server=db7874.public.databaseasp.net; Database=db7874; User Id=db7874; Password=F!c97B#gn_4Q; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;";
        //readonly string connectionString = "Server=TRIPLETS\\SQLEXPRESS; Database=FirstDapperProjectDB; Trusted_Connection=True; MultipleActiveResultSets=True; Encrypt=false;";

        public GenericRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> result;
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT {GetColumnsAsProperties()} FROM {tableName}";

                result = await _connection.QueryAsync<T>(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching records from db: ${ex.Message}");
                throw new Exception("Unable to fetch data. Please contact the administrator.");
            }
            finally
            {
                _connection.Close();
            }
            return result;
        }

        public async Task<bool> Delete(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string? tableName = GetTableName();
                string? keyColumn = GetKeyColumnName();
                string? keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";

                rowsEffected = await _connection.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting a record in db: ${ex.Message}");
                rowsEffected = -1;
            }
            finally
            {
                _connection.Close();
            }

            return rowsEffected > 0 ? true : false;
        }


        public async Task<bool> Update(T entity)
        {

            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");

                var properties = GetProperties(true); // Exclude key property

                if (properties == null || !properties.Any())
                {
                    throw new InvalidOperationException("No properties found to update.");
                }

                foreach (var property in properties)
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    string propertyName = property.Name;
                    string columnName = columnAttribute?.Name ?? propertyName;
                    query.Append($"{columnName} = @{propertyName},");
                }

                // Remove the last comma
                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                // Execute the update query
                var affectedRows = await _connection.ExecuteAsync(query.ToString(), entity);

                // Check if any rows were affected
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating record in db: {ex.Message}");
                throw; // Rethrow exception to be handled by the caller
            }
            finally
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
        }

        public async Task<T> GetById(int id)
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName(); // Corrected: Use GetKeyColumnName() to get the correct column name
                string query = $"SELECT {GetColumnsAsProperties()} FROM {tableName} WHERE {keyColumn} = @Id";

                // Parameterize the query to prevent SQL injection
                result = await _connection.QueryAsync<T>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching record: {ex.Message}");
                return null; // Return null if there's an error
            }

            return result?.FirstOrDefault();
        }

        public bool Add(T entity)
        {
            string tableName = GetTableName();
            string columns = GetColumnNames(true);
            string values = GetColumnValues(true);
            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";


            int affectedRows = 0;

            affectedRows = _connection.Execute(query, entity);

            return affectedRows == 1;
        }
        // Function on getting Name
        public string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;

            }

            return tableName;
        }

        //Function Column
        public string GetColumnNames(bool excludeKey = false)
        {
            string columnNames = "";
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;

                }
                ));
            return columns;


        }
        public string GetColumnValues(bool excludeKey = false)
        {
            var columnValues = typeof(T).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute(typeof(KeyAttribute)) == null);
            var values = string.Join(", ", columnValues.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;

        }
        private string GetColumnsAsProperties(bool excludeKey = false)
        {
            var type = typeof(T);
            var columnsAsProperties = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
                .Select(p =>
                {
                    var columnAttribute = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttribute != null ? $"{columnAttribute.Name} AS {p.Name}" : p.Name;
                }));

            return columnsAsProperties;
        }

        private static string? GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute?.Name ?? "";
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        private string? GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToList();

            if (properties.Any())
                return properties?.FirstOrDefault()?.Name ?? null;

            return null;
        }

        private IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

            return properties;
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
