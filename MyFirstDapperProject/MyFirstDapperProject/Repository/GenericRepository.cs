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
        readonly string connectionString = "Server=TRIPLETS\\SQLEXPRESS; Database=FirstDapperProjectDB; Trusted_Connection=True; MultipleActiveResultSets=True; Encrypt=false";

        public GenericRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public T GetById(int id)
        {
            return null;
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
    }
}
