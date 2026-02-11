using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Instructor;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet("{instructorId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<InstructorResponseDto>>> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            var instructor = await _instructorService.GetInstructorDetailsByInstructorIdAsync(instructorId);

            return Ok(new ApiResponse<InstructorResponseDto>
            {
                Success = true,
                Data = instructor,
                Message = $"Instructor by Id {instructorId} retrieved successfully ..."
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<InstructorResponseDto>>> GetAllInstructorsAsync()
        {
            var allInstructors = await _instructorService.GetAllInstructorsAsync();
            return Ok(new ApiResponse<IEnumerable<InstructorResponseDto>>
            {
                Success = true,
                Data = allInstructors,
                Message = "Instructors retrieved successfully ..."
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateInstructorAsync([FromBody] CreateInstructorDto dto)
        {
            var result = await _instructorService.CreateInstructorAsync(dto);
            return Ok($"Successfully created with Id {result}");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<InstructorResponseDto>>> UpdateInstructorDetailsAsync([FromBody] UpdateInstructorDto dto)
        {
            var updatedInstructor = await _instructorService.UpdateInstructorDetailsAsync(dto);

            return Ok(new ApiResponse<InstructorResponseDto>
            {
                Success = true,
                Data = updatedInstructor,
                Message = "Instructor updated successfully ..."
            });
        }

        [HttpPut("{instructorId}/Inactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> InactivateInstructorByInstructorIdAsync(int instructorId)
        {
            await _instructorService.InactivateInstructorByInstructorIdAsync(instructorId);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Instructor inactivated successfully ..."
            });
        }
    }
}
