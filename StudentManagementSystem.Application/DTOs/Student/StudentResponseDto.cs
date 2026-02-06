using StudentManagementSystem.Application.DTOs.Course;

namespace StudentManagementSystem.Application.DTOs.Student
{
    public class StudentResponseDto
    {
        public int StudentId { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? NIC { get; set; }
        public ICollection<CourseDto> EnrolledCourses { get; set; }

    }
}
