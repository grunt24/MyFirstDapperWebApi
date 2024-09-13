using MyFirstDapperProject.Model;
using MyFirstDapperProject.Repository;
using MyFirstDapperProject.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstDapperProject.Service
{
    public class CourseService
    {
        CourseRepository _courseRepository = new CourseRepository();

        public async Task<IEnumerable<CourseModel>> GetAll()
        {
            return await _courseRepository.GetAll();
        }

        public bool AddStudent(CourseModel course)
        {
            return _courseRepository.Add(course);
        }

        public async Task<bool> Update(CourseModel courseModel)
        {
            return await _courseRepository.Update(courseModel);
        }

        public async Task<CourseModel> GetById(int id)
        {
            return await _courseRepository.GetById(id);
        }

        public async Task<bool> DeleteCourse(CourseModel courseModel)
        {
            return await _courseRepository.Delete(courseModel);
        }

    }
}
