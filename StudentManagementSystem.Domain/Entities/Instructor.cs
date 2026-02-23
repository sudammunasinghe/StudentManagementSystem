namespace StudentManagementSystem.Domain.Entities
{
    public class Instructor : SystemUser
    {
        public int ExperienceYears { get; set; }
        public decimal? PreferredSalary { get; set; }
        public bool? IsApproved { get; set; }

        private Instructor() { }

        public static Instructor Create(
            int experienceYears,
            decimal? preferredSalary
            )
        {
            return new Instructor
            {
                ExperienceYears = experienceYears,
                PreferredSalary = preferredSalary
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
