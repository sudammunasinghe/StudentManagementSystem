using StudentManagementSystem.Application.DTOs.CourseContent;

namespace StudentManagementSystem.Application.DTOs.Course
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string? Title { get; set; }
        public int? Credits { get; set; }
        public string? EnrollmentStatus { get; set; }
        public int? NoOfEnrolledStudents { get; set; }
        public ICollection<CourseContentDto>? CourseContents { get; set; }
    }
}
