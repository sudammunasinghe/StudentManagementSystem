using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("{courseId}/Enroll")]
        public async Task<ActionResult<ApiResponse<string>>> EnrollToCourseAsync(int courseId)
        {
            await _studentService.EnrollToCourseAsync(courseId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Enrollemnt Successfull ..."
            });
        }

        [HttpGet("EnrolledCourses")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetAllEnrolledCoursesAsync()
        {
            var response = await _studentService.GetAllEnrolledCoursesAsync();
            return Ok(new ApiResponse<IEnumerable<CourseDto>>
            {
                Success = true,
                Data = response,
                Message = "Enrolled courses retrieved successfully ..."
            });
        }
    }
}
