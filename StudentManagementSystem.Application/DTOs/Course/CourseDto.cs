using StudentManagementSystem.Application.DTOs.CourseContent;

namespace StudentManagementSystem.Application.DTOs.Course
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public int? Credits { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryEnum { get; set; }
        public int? DurationHours { get; set; }
        public int? EntrollmentLimit { get; set; }
        public ICollection<CourseContentDto>? CourseContents { get; set; }
    }
}
