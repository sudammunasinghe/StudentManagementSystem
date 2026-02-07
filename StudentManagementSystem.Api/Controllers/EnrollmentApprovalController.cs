using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentApprovalController : ControllerBase
    {
        private readonly IEnrollmentApprovalService _enrollmentApprovalService;
        public EnrollmentApprovalController(IEnrollmentApprovalService enrollmentApprovalService)
        {
            _enrollmentApprovalService = enrollmentApprovalService;
        }

        [HttpGet]
        public async Task<ActionResult<PendingApprovalDetailsDto>> GetEnrollmentPendingApprovals()
        {
            var response = await _enrollmentApprovalService.GetEnrollmentPendingApprovals();
            return Ok(response);
        }

        [HttpPut("{studentId}/{courseId}/status")]
        public async Task<ActionResult> ManageApprovalWorkFlowAsync(int studentId, int courseId, [FromBody] ApprovalCompletionDto dto)
        {
            var result = await _enrollmentApprovalService.ManageApprovalWorkFlowAsync(studentId, courseId, dto);
            return result switch
            {
                ApprovalResult.Approved => Ok("Successfully Approved ..."),
                ApprovalResult.Rejected => Ok("Successfully Rejected ..."),
                ApprovalResult.NotFound => Ok("Enrollment Not Found ..."),
                _ => BadRequest("Invalid Status Transition ...")
            };
        }
    }
}
