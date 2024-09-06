using MyFirstDapperProject.Model;
using MyFirstDapperProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstDapperProject.Service
{
    public class StudentService 
    {
        StudentRepository _studentRepository;

        public StudentModel GetStudentById(int id)
        {
            _studentRepository = new StudentRepository();
            return _studentRepository.GetById(id);
        }

        public bool AddStudent(StudentModel student)
        {
            _studentRepository = new StudentRepository();
            return _studentRepository.Add(student);
        }

        //public StudentModel 
    }
}
