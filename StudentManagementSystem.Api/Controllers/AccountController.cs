using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.Account;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<ApiResponse<ProfileDetailsDto>>> GetProfileDetailsAsync()
        {
            var response = await _accountService.GetProfileDetailsAsync();
            return Ok(new ApiResponse<ProfileDetailsDto>
            {
                Success = true,
                Data = response,
                Message = "Profile details retrieved successfully ..."
            });
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateProfileDetailsAsync([FromBody] UpdateProfileDetailsDto dto)
        {
            await _accountService.UpdateProfileDetailsAsync(dto);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Profile data updated successfully ..."
            });
        }
    }
}
