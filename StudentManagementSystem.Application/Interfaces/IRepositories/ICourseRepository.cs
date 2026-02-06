using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface ICourseRepository
    {
        Task<Course?> GetCourseDetailsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<int> CreateNewCourseAsync(Course newCourse);
        Task<int> UpdateCourseDetailsAsync(Course updatedCourse);
        Task<int> InactivateCourseByCourseIdAsync(int courseId);
        Task<int> GetEnrolledStudentCountForCourseByCourseIdAsync(int courseId);
    }
}
