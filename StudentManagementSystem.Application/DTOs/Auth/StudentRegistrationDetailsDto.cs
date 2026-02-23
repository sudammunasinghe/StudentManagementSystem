namespace StudentManagementSystem.Application.DTOs.Auth
{
    public class StudentRegistrationDetailsDto : RegistrationDetailsDto
    {
        public double? GPA { get; set; }
        public ICollection<CreateEducationalDetailsDto>? EducationalDetails { get; set; }
    }
}
