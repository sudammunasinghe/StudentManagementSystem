using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewStudentAsync([FromBody] StudentRegistrationDetailsDto dto)
        {
            var result = await _authService.RegisterNewStudentAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        [HttpPost("register/instructor")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> RegisterNewInstructorAsync([FromBody] InstructorRegistrationDetailsDto dto)
        {
            var result = await _authService.RegisterNewInstructorAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> Login(string email, string password)
        {
            var token = await _authService.LoginAsync(email, password);
            return Ok(new { TokenId = token });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> ForgotPasswordAsync([FromForm] ForgotPasswordDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> ResetPasswordAsync([FromForm] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<string>>> ChangePasswordAsync([FromForm] ChangePasswordDto dto)
        {
            var result = await _authService.ChangePasswordAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = result
            });
        }
    }
}
