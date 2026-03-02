namespace StudentManagementSystem.Application.DTOs.Approval
{
    public class EnrollmentApprovalCompletionDto : ApprovalCompletionDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
