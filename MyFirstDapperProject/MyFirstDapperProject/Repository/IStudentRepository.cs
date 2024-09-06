using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyFirstDapperProject.Repository.Interface;

namespace MyFirstDapperProject.Repository
{
    public interface IStudentRepository : IGenericRepository<StudentRepository>
    {
    }
}
