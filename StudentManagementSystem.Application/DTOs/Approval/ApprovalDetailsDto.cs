namespace StudentManagementSystem.Application.DTOs.Approval
{
    public class ApprovalDetailsDto
    {
        public string? RegistrationNumber { get; set; }
        public string? ApprovalType { get; set; }
        public int? StudentId { get; set; }
        public int? EnrolledCourseId { get; set; }
        public int? InstructorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? RoleId { get; set; }
        public string? ApprovalStatus { get; set; }
        public string? RejectedReason { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public DateTime? ApprovedDateTime { get; set; }

    }
}
