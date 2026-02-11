using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Exceptions;
using System.Linq.Expressions;

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

        [HttpGet("{stdId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> GetStudentDetailsByStudentIdAsync(int stdId)
        {
            var student = await _studentService.GetStudentDetailsByStudentIdAsync(stdId);
            return Ok(new ApiResponse<StudentResponseDto>
            {
                Success = true,
                Data = student,
                Message = $"Student by Id {stdId} retrieved successfully ..."
            });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<StudentResponseDto>>>> GetAllStudentsAsync()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(new ApiResponse<IEnumerable<StudentResponseDto>>
            {
                Success = true,
                Data = students,
                Message = "Students retrieved successfully ..."
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> CreateStudentAsync(CreateStudentDto dto)
        {
            var studentId = await _studentService.CreateStudentAsync(dto);
            var newStudent = await _studentService.GetStudentDetailsByStudentIdAsync(studentId);
            return CreatedAtAction(
                nameof(GetStudentDetailsByStudentIdAsync),
                new { stdId = studentId },
                new ApiResponse<StudentResponseDto>
                {
                    Success = true,
                    Data = newStudent,
                    Message = "Student successfully created ..."
                });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<StudentResponseDto>>> UpdateStudentDetailsAsync(UpdateStudentDto dto)
        {
            var updatedStudent = await _studentService.UpdateStudentDetailsAsync(dto);

            return Ok(new ApiResponse<StudentResponseDto>
            {
                Success = true,
                Data = updatedStudent,
                Message = "Student successfully updated ..."
            });
        }

        [HttpPut("{stdId}/Incativate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<string>>> InactivateStudentByStudentIdAsync(int stdId)
        {
            await _studentService.InactivateStudentByStudentIdAsync(stdId);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Student successfully inactivated ..."
            });
        }
    }
}
