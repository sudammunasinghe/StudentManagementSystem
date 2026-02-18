namespace StudentManagementSystem.Application.DTOs.CourseContent
{
    public class UploadCourseContentDto
    {
        public int? CourseId { get; set; }
        public int? InstructorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Stream? FileStream { get; set; }
        public string? FileName { get; set; }
    }
}
