namespace StudentManagementSystem.Application.DTOs.Account
{
    public class ProfileDetailsDto
    {
        public string? RegistrationNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? NIC { get; set; }
        public ICollection<EducationalDetailsDto>? EducationalDetails { get; set; }
        public ICollection<InstructorExperienceDetailsDto>? InstructorExperienceDetails { get; set; }
    }
}
