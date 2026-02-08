using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.Instructor;
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
        public async Task<IActionResult> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            var instructor = await _instructorService.GetInstructorDetailsByInstructorIdAsync(instructorId);
            if (instructor == null)
                return NotFound(new { Success = false, Message = "Instructor not found ..." });

            return Ok(new
            {
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateInstructorDetailsAsync([FromBody] UpdateInstructorDto dto)
        {
            try
            {
                var result = await _instructorService.UpdateInstructorDetailsAsync(dto);
                if (!result)
                    return NotFound(new { Success = false, Message = "Instructor not found ..." });

                var updatedInstructor = await _instructorService.GetInstructorDetailsByInstructorIdAsync(dto.Id);
                return Ok(new
                {
                    Success = true,
                    Data = updatedInstructor,
                    Message = "Instructor updated successfully ..."
                });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("{instructorId}/Inactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InactivateInstructorByInstructorIdAsync(int instructorId)
        {
            try
            {
                var result = await _instructorService.InactivateInstructorByInstructorIdAsync(instructorId);
                if (!result)
                    return NotFound(new { Success = false, Message = "Instructor not found ..." });
                return Ok(new { Success = true, Message = "Successfully Inactivated ..." }); ;
            }
            catch (DomainException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}
