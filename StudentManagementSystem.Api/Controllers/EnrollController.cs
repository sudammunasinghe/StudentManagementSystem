using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Application.DTOs.Enroll;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollController : ControllerBase
    {
        private readonly IEnrollService _enrollService;
        public EnrollController(IEnrollService enrollService)
        {
            _enrollService = enrollService;
        }

        [HttpPost]
        public async Task<ActionResult> EnrollStudentAsync(EnrollStudentDto dto)
        {
            await _enrollService.EnrollStudentAsync(dto);
            return Ok("Enrollment Approval Initiated ...");
        }
    }
}
