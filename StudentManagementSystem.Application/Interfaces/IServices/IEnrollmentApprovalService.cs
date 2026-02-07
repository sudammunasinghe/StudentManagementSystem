using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IEnrollmentApprovalService
    {
        Task<IEnumerable<PendingApprovalDetailsDto>> GetEnrollmentPendingApprovals();
        Task<ApprovalResult> ManageApprovalWorkFlowAsync(int studentId,int courseId, ApprovalCompletionDto dto);
    }
}
