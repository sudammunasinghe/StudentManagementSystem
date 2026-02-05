using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<CourseDto>> GetCourseDetailsByCourseIdAsync(int courseId)
        {
            var response = await _courseService.GetCourseDetailsByCourseIdAsync(courseId);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCoursesAsync()
        {
            var response = await _courseService.GetAllCoursesAsync();
            return Ok(response);
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
        public async Task<ActionResult> UpdateCourseDetailsAsync(UpdateCourseDto dto)
        {
            var result = await _courseService.UpdateCourseDetailsAsync(dto);
            return Ok(result);
        }

        [HttpPut("{courseId}/Inactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> InactivateCourseByCourseIdAsync(int courseId)
        {
            var result = await _courseService.InactivateCourseByCourseIdAsync(courseId);
            return Ok(result);
        }
    }
}
