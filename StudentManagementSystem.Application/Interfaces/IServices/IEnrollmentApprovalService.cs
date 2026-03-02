using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IEnrollmentApprovalService
    {
        Task<IEnumerable<ApprovalDetailsDto>> GetEnrollmentPendingApprovals();
        Task<ApprovalResult> ManageApprovalWorkFlowAsync(int studentId, int courseId, ApprovalCompletionDto dto);
    }
}
