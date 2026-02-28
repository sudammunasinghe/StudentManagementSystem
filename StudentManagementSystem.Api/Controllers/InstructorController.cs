using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(Roles.Instructor))]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CourseDto>>>> GetCoursesByInstructorAsync()
        {
            var courseDetails = await _instructorService.GetCoursesByInstructorAsync();
            return Ok(new ApiResponse<IEnumerable<CourseDto>>
            {
                Success = true,
                Message = "Course details retrieved successfully ...",
                Data = courseDetails
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreateNewCourseAsync(CreateCourseDto dto)
        {
            await _instructorService.CreateNewCourseAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Course created succefully"
            });
        }

        [HttpPost("courseContent")]
        public async Task<ActionResult<ApiResponse<string>>> UploadCourseContentAsync([FromForm] UploadCourseContentRequest request)
        {
            using var stream = request.File.OpenReadStream();
            var dto = new UploadCourseContentDto
            {
                CourseId = request.CourseId,
                Title = request.Title,
                Description = request.Description,
                FileStream = stream,
                FileName = request.File.FileName
            };
            await _instructorService.UploadCourseContentAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Course content uploaded successfully ..."
            });
        }
    }
}
