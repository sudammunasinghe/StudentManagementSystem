using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IInstructorRepository
    {
        Task<(List<Course>? courses, List<CourseContent>? contents)> GetCoursesByInstructorAsync(int instructorId);
        Task<Instructor?> GetInstructorDetailsByUserIdAsync(int userId);
        Task CreateNewCourseAsync(Course newCourse);
        Task<Course?> GetCourseDetailsByCourseIdAsync(int courseId);
        Task UploadCourseContentAsync(CourseContent newContent);
        Task InactivateCourseByCourseIdAsync(int courseId);
        Task InactivateCourseContentByContentIdAsync(int contentId);
        Task<CourseContent?> GetCourseContentByContentIdAsync(int contentId);
    }
}
