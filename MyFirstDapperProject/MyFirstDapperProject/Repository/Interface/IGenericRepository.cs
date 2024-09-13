using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstDapperProject.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {

        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        bool Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
