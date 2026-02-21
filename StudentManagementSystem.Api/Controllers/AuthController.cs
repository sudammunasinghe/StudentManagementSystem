using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Auth;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/student")]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewStudentAsync([FromForm] StudentRegistrationDetailsDto dto)
        {
            var result = await _authService.RegisterNewStudentAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        [HttpPost("register/instructor")]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewInstructorAsync([FromForm] InstructorRegistrationDetailsDto dto)
        {
            var result = await _authService.RegisterNewInstructorAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login(string email, string password)
        {
            var token = await _authService.LoginAsync(email, password);
            return Ok(new { TokenId = token });
        }
    }
}
