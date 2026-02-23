namespace StudentManagementSystem.Application.DTOs.Auth
{
    public class CreateInstructorExperienceDetailsDto
    {
        public string? CompanyName { get; set; }
        public string? JobTitle { get; set; }
        public string? EmployementType { get; set; }
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrentlyWorking { get; set; }
        public string? Description { get; set; }
    }
}
