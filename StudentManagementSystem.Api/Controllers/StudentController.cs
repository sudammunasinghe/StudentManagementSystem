using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.Student;
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
        [HttpGet("{stdId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentResponseDto>> GetStudentDetailsByStudentIdAsync(int stdId)
        {
            var response = await _studentService.GetStudentDetailsByStudentIdAsync(stdId);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetAllStudentsAsync()
        {
            var response = await _studentService.GetAllStudentsAsync();
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateStudentAsync(CreateStudentDto dto)
        {
            var result = await _studentService.CreateStudentAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateStudentDetailsAsync(UpdateStudentDto dto) 
        {
            var result = await _studentService.UpdateStudentDetailsAsync(dto);
            return Ok(result);
        }

        [HttpPut("{stdId}/Incativate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> IncativateStudentByStudentIdAsync(int stdId)
        {
            var result = await _studentService.IncativateStudentByStudentIdAsync(stdId);
            return Ok(result);
        }
    }
}
