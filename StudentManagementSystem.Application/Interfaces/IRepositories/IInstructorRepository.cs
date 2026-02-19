using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IInstructorRepository
    {
        Task<Instructor?> GetInstructorDetailsByInstructorIdAsync(int? instructorId);
        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
        Task<int> CreateInstructorAsync(Instructor newInstructor);
        Task<int> UpdateInstructorDetailsAsync(Instructor updatedInstructor);
        Task<int> InactivateInstructorByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Course>> GetOwnCoursesByInstructorIdAsync(int instructorId);
        Task<IEnumerable<CourseContent>> GetCourseContentByInstructorIdAsync(int instructorId);
    }
}
