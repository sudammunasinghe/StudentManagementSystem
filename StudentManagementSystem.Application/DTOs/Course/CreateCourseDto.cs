namespace StudentManagementSystem.Application.DTOs.Course
{
    public class CreateCourseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryEnum { get; set; }
        public int? Credits { get; set; }
        public int? DurationHours { get; set; }
        public int? EntrollmentLimit { get; set; }
    }
}
