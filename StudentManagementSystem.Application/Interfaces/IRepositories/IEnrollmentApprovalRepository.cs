using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IEnrollmentApprovalRepository
    {
        Task<IEnumerable<PendingApprovalDetailsDto>> GetEnrollmentPendingApprovals();
        Task<Enrollment> GetEnrollmentDetailsAsync(int studentId, int courseId);
        Task<int> ManageApprovalWorkFlowAsync(Enrollment approvalDetails);
    }
}
