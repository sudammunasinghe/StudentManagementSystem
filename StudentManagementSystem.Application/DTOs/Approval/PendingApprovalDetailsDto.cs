namespace StudentManagementSystem.Application.DTOs.Approval
{
    public class PendingApprovalDetailsDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string FullName { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime RequestedDateTime { get; set; }

    }
}
