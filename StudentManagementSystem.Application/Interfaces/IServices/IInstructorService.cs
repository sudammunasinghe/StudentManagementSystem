using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IInstructorService
    {
        Task<IEnumerable<CourseDto>> GetCoursesByInstructorAsync();
        Task CreateNewCourseAsync(CreateCourseDto dto);
        Task UploadCourseContentAsync(UploadCourseContentDto dto);
        Task InactivateCourseByCourseIdAsync(int courseId);
        Task InactivateCourseContentByContentIdAsync(int contentId);
    }
}
