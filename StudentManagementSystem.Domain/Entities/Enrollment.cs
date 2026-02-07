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
            if (Status != EnrollmentStatus.Pending)
                throw new Exception("Only pending enrollment can be approved ...");
        }
        public void Reject(string? reason)
        {
            if (Status != EnrollmentStatus.Pending)
                throw new Exception("Only pending enrollment can be rejected ...");

            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Rejected reason is mandatory ...");
        }
    }
}
