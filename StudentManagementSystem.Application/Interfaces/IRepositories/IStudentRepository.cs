using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByUserIdAsync(int userId);
        Task EnrollToCourseAsync(int studentId, int courseId);
        Task<Enrollment?> GetEnrollmentDetailsAsync(int studentId, int courseId);
    }
}
