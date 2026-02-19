using StudentManagementSystem.Application.DTOs.Course;

namespace StudentManagementSystem.Application.DTOs.Instructor
{
    public class InstructorResponseDto
    {
        public int InstructorId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public ICollection<CourseDto> OwnCourses { get; set; }

    }
}
