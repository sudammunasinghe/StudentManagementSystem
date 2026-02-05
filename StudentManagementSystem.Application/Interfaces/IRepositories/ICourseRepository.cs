using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface ICourseRepository
    {
        Task<Course?> GetCourseDetailsByCourseIdAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<int> CreateNewCourseAsync(Course newCourse);
        Task<int> UpdateCourseDetailsAsync(Course updatedCourse);
        Task<int> InactivateCourseByCourseIdAsync(int courseId);
    }
}
