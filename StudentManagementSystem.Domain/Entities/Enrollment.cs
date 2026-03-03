using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public EnrollmentStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime ApprovedAt { get; set; }
        public string? RejectedReason { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public void Approve()
        {
            Status = EnrollmentStatus.Approved;
        }
        public void Reject(string? reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new DomainException("Rejected reason is mandatory ...");
            Status = EnrollmentStatus.Rejected;
            RejectedReason = reason;
        }
    }
}
