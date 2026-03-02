namespace StudentManagementSystem.Domain.Entities
{
    public class ProfileEntity
    {
        public int? Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? NIC { get; set; }
        public List<Education>? EducationalDetails { get; set; }
        public List<InstructorExperience>? InstructorExperiences { get; set; }
    }
}
