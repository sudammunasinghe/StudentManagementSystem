using StudentManagementSystem.Application.DTOs.Course;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ICourseService
    {
        Task<CourseDto> GetCourseDetailsByCourseIdAsync(int courseId);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
        Task<int> CreateNewCourseAsync(CreateCourseDto dto);
        Task<CourseDto> UpdateCourseDetailsAsync(UpdateCourseDto dto);
        Task InactivateCourseByCourseIdAsync(int courseId);
    }
}
