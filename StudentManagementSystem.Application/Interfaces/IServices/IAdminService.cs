using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IAdminService
    {
        Task<IEnumerable<ApprovalDetailsDto>> GetApprovalDetailsAsync(EnrollmentStatus? status);
        Task CompleteStudentEnrollmentApprovalAsync(EnrollmentApprovalCompletionDto enrollmentApproval);
        Task CompleteInstructorRegistrationApprovalAsync(InstructorRegeistrationApprovalCompletionDto instructorApproval);
    }
}
