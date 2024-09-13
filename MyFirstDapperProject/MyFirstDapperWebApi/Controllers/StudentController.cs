using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstDapperProject.Model;
using MyFirstDapperProject.Service;
using System.Runtime.CompilerServices;

namespace MyFirstDapperWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly StudentService _studentService = new StudentService();

        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetAllStudent()
        {
            var students = await _studentService.GetAll();
            return Ok(students);
        }

        [HttpPost]
        public bool AddStudent(StudentModel student)
        {
            return _studentService.AddStudent(student);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudentTBL(int id, [FromBody] StudentModel studentModel)
        {
            try
            {
                // Ensure that the ID from the URL matches the student's ID.
                if (id != studentModel.StudentId)
                    return BadRequest("ID mismatch.");

                var existingStudent = await _studentService.GetById(id);
                if (existingStudent == null)
                    return NotFound();

                // Perform the update operation.
                var updateSuccessful = await _studentService.Update(studentModel);
                if (!updateSuccessful)
                    return NotFound();

                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the student: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var studentModel = new StudentModel { StudentId = id };

                bool result = await _studentService.DeleteStudent(studentModel);
                if (result)
                {
                    return Ok("Deleted Successfully");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the student: {ex.Message}");
            }
        }
    }
}
