using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface ICourseContentRepository
    {
        Task UploadCourseContentAsync(CourseContent newContent);
    }
}
