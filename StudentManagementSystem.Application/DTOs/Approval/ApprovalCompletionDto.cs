using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Application.DTOs.Approval
{
    public class ApprovalCompletionDto
    {
        public EnrollmentStatus Status { get; set; }
        public string? RejectedReason { get; set; }
    }
}
