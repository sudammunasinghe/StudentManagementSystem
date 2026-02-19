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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> UploadCourseContentAsync([FromForm] UploadCourseContentRequest request)
        {
            using var stream = request.File.OpenReadStream();

            var dto = new UploadCourseContentDto
            {
                CourseId = request.CourseId,
                InstructorId = request.InstructorId,
                Title = request.Title,
                Description = request.Description,
                FileStream = stream,
                FileName = request.File.FileName
            };
            await _courseContentService.UploadCourseContentAsync(dto);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Content uplaoded successfully ..."
            });
        }

        [HttpPut("{courseContentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<string>>> UpdateCourseMetaDataAsync(int courseContentId, UpdateCourseMetaDataDto dto)
        {
            await _courseContentService.UpdateCourseMetaDataAsync(courseContentId, dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Meta data updated successfully ..."
            });
        }

        [HttpPut("{courseContentId}/inactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> InactivateCourseContentByCourseContentIdAsync(int? courseContentId)
        {
            await _courseContentService.InactivateCourseContentByCourseContentIdAsync(courseContentId);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Course content successfully inactivated ..."
            });
        }
    }

}
