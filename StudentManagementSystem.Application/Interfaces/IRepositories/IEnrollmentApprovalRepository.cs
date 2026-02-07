using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IEnrollmentApprovalRepository
    {
        Task<IEnumerable<PendingApprovalDetailsDto>> GetEnrollmentPendingApprovals();
        Task<Enrollment> GetEnrollmentDetailsAsync(int studentId, int courseId);
        Task<int> ManageApprovalWorkFlowAsync(Enrollment approvalDetails);
    }
}
