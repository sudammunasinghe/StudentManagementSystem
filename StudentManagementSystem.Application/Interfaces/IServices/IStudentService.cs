using StudentManagementSystem.Application.DTOs.Course;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IStudentService
    {
        Task EnrollToCourseAsync(int courseId);
        Task<IEnumerable<CourseDto>> GetAllEnrolledCoursesAsync();
    }
}
