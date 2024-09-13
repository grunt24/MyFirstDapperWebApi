using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstDapperProject.Model;
using MyFirstDapperProject.Service;

namespace MyFirstDapperWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly CourseService _courseService = new CourseService();

        [HttpGet("GetCourses")]
        public async Task<ActionResult<IEnumerable<CourseModel>>> GetAllStudent()
        {
            var courses = await _courseService.GetAll();
            return Ok(courses);
        }

        [HttpPost]
        public bool AddStudent(CourseModel data)
        {
            return _courseService.AddStudent(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseModel courseModel)
        {
            try
            {
                if (id != courseModel.CourseId)
                    return BadRequest("ID mismatch.");

                var existingCourse = await _courseService.GetById(id);
                if (existingCourse == null)
                    return NotFound();

                var updateSuccessful = await _courseService.Update(courseModel);
                if (!updateSuccessful)
                    return NotFound();

                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the course: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var courseModel = new CourseModel { CourseId = id };

                bool result = await _courseService.DeleteCourse(courseModel);
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
