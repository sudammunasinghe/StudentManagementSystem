using StudentManagementSystem.Application.DTOs.Course;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<CourseDto> GetCourseDetailsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<int> CreateNewCourseAsync(CreateCourseDto dto);
        Task<bool> UpdateCourseDetailsAsync(UpdateCourseDto dto);
        Task<bool> InactivateCourseByCourseIdAsync(int courseId);
    }
}
