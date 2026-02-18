using StudentManagementSystem.Application.DTOs.CourseContent;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ICourseContentService
    {
        Task UploadCourseContentAsync(UploadCourseContentDto dto);
    }
}
