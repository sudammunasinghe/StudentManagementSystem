namespace StudentManagementSystem.Application.DTOs.Auth
{
    public class InstructorRegistrationDetailsDto : RegistrationDetailsDto
    {
        public int ExperienceYears { get; set; }
        public decimal? PreferredSalary { get; set; }

    }
}
