using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ApprovalDetailsDto>> GetAllApprovalDetailsAsync(int? statusCode);
        Task<Enrollment?> GetEnrollmentDetailsAsync(int studentId, int courseId);
        Task<int> CompleteStudentEnrollmentApprovalAsync(Enrollment updatedEnrollment);
        Task<Instructor?> GetInstructorDetailsByInstructorIdAsync(int instructorId);
        Task<int> CompleteInstructorRegistrationApprovalAsync(Instructor updatedInstructor);
    }
}
