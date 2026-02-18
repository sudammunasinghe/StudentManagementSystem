using StudentManagementSystem.Application.DTOs.CourseContent;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ICourseContentService
    {
        Task UploadCourseContentAsync(UploadCourseContentDto dto);
        Task UpdateCourseMetaDataAsync(int contentId, UpdateCourseMetaDataDto dto);
        Task InactivateCourseContentByCourseContentIdAsync(int? contentId);
    }
}
