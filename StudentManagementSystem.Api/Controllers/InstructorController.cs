using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.Instructor;
using StudentManagementSystem.Application.Interfaces.IServices;

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
        public async Task<IActionResult> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            var instructor = await _instructorService.GetInstructorDetailsByInstructorIdAsync(instructorId);
            if (instructor == null)
                return NotFound(new { Success = false, Message = "Instructor not found ..."});

            return Ok( new { 
                Success = true,
                Data = instructor,
                Message = $"Instructor by Id {instructorId} retrieved successfully ..."
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllInstructorsAsync()
        {
            try
            {
                var instructors = await _instructorService.GetAllInstructorsAsync();
                return Ok(new
                {
                    Success = true,
                    Data = instructors,
                    Message = "Instructors retrieved successfully ..."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateInstructorAsync([FromBody] CreateInstructorDto dto)
        {
            var result = await _instructorService.CreateInstructorAsync(dto);
            return Ok($"Successfully created with Id {result}");
        }
    }
}
