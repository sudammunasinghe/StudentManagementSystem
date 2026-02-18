using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface ICourseContentRepository
    {
        Task<CourseContent?> GetCourseContentByCourseContentIdAsync(int? contentId);
        Task UploadCourseContentAsync(CourseContent newContent);
        Task<int> UpdateCourseMetaDataAsync(CourseContent updatedContent);
        Task<int> InactivateCourseContentByCourseContentIdAsync(int? contentId);
    }
}
