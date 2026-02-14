namespace StudentManagementSystem.Application.DTOs.Course
{
    public class CreateCourseDto
    {
        public string? Title { get; set; }
        public int? Credits { get; set; }
        public int? InstructorId { get; set; }
    }
}
