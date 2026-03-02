using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.ApiResponse;
using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ApprovalDetailsDto>>>> GetOwnApprovalsAsync(int? status)
        {
            var response = await _adminService.GetApprovalDetailsAsync((EnrollmentStatus?)status);
            return Ok(new ApiResponse<IEnumerable<ApprovalDetailsDto>>
            {
                Success = true,
                Data = response,
                Message = "Approval details retrieved successfully ..."
            });
        }

        [HttpPut("EnrollmentApproval")]
        public async Task<ActionResult<ApiResponse<string>>> CompleteStudentEnrollmentApprovalAsync([FromForm] EnrollmentApprovalCompletionDto enrollmentApproval)
        {
            await _adminService.CompleteStudentEnrollmentApprovalAsync(enrollmentApproval);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Approval status updated successfully ..."
            });
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<string>>> CompleteInstructorRegistrationApprovalAsync([FromForm] InstructorRegeistrationApprovalCompletionDto instructorApproval)
        {
            await _adminService.CompleteInstructorRegistrationApprovalAsync(instructorApproval);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Approval status updated successfully ..."
            });
        }
    }
}
