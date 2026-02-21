using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourseDetailsByCourseIdAsync(int courseId)
        {
            var course = await _courseService.GetCourseDetailsByCourseIdAsync(courseId);

            return Ok(new ApiResponse<CourseDto>
            {
                Success = true,
                Data = course,
                Message = $"Course by Id {courseId} retrieved successfully ..."
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetAllCoursesAsync()
        {
            var allCourses = await _courseService.GetAllCoursesAsync();
            return Ok(new ApiResponse<IEnumerable<CourseDto>>
            {
                Success = true,
                Data = allCourses,
                Message = "Courses retrieved successfully ..."
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateNewCourseAsync(CreateCourseDto dto)
        {
            var result = await _courseService.CreateNewCourseAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourseDetailsAsync(UpdateCourseDto dto)
        {
            var updatedCourse = await _courseService.UpdateCourseDetailsAsync(dto);
            return Ok(new ApiResponse<CourseDto>
            {
                Success = true,
                Data = updatedCourse,
                Message = "Course updated successfully ..."
            });
        }

        [HttpPut("{courseId}/Inactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> InactivateCourseByCourseIdAsync(int courseId)
        {
            await _courseService.InactivateCourseByCourseIdAsync(courseId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Course successfully inactivated ..."
            });
        }
    }
}
