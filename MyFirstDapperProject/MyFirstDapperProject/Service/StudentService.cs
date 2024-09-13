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
        StudentRepository _studentRepository = new StudentRepository();

        public async Task<IEnumerable<StudentModel>> GetAll()
        {
            return await _studentRepository.GetAll();
        }

        public bool AddStudent(StudentModel student)
        {   
            return _studentRepository.Add(student);
        }

        public async Task<bool> Update(StudentModel data)
        {
                return await _studentRepository.Update(data);
        }

        public async Task<StudentModel> GetById(int id)
        {
            return await _studentRepository.GetById(id);
        }

        public async Task<bool> DeleteStudent(StudentModel studentModel)
        {
            return await _studentRepository.Delete(studentModel);
        }
    }

}
