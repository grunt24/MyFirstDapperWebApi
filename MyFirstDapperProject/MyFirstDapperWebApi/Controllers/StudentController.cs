using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstDapperProject.Model;
using MyFirstDapperProject.Service;

namespace MyFirstDapperWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpPost]
        public bool AddStudent(StudentModel student)
        {
            StudentService studentService = new StudentService();
            return studentService.AddStudent(student);
        }
    }
}
