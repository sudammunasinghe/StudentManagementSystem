namespace StudentManagementSystem.Domain.Entities
{
    public class Instructor : SystemUser
    {
        public int ExperienceYears { get; set; }
        public decimal? PreferredSalary { get; set; }
        public string RejectedReason { get; set; }
        public EnrollmentStatus? Status { get; set; }
        public List<InstructorExperience>? InstructorExperiences { get; set; }

        private Instructor() { }

        public void Approve()
        {
            Status = EnrollmentStatus.Approved;
        }
        public void Reject(string? reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new Exception("Rejected reason is mandatory ...");
            Status = EnrollmentStatus.Rejected;
            RejectedReason = reason;
        }

        public static Instructor Create(
            int experienceYears,
            decimal? preferredSalary
            )
        {
            return new Instructor
            {
                ExperienceYears = experienceYears,
                PreferredSalary = preferredSalary,
                InstructorExperiences = new List<InstructorExperience>()
            };
        }

        public void Update(
            int experienceYears,
            decimal? preferredSalary
            )
        {

        }
    }
}
