using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseContentController : ControllerBase
    {
        private readonly ICourseContentService _courseContentService;
        public CourseContentController(ICourseContentService courseContentService)
        {
            _courseContentService = courseContentService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> UploadCourseContentAsync([FromForm] UploadCourseContentRequest request)
        {
            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);

            var dto = new UploadCourseContentDto
            {
                CourseId = request.CourseId,
                InstructorId = request.InstructorId,
                Title = request.Title,
                Description = request.Description,
                FileBytes = ms.ToArray(),
                FileName = request.File.FileName
            };
            await _courseContentService.UploadCourseContentAsync(dto);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Content uplaoded successfully ..."
            });
        }
    }

}
