using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Application.Services
{
    public class EnrollmentApprovalService : IEnrollmentApprovalService
    {
        private readonly IEnrollmentApprovalRepository _enrollmentApprovalRepository;
        public EnrollmentApprovalService(IEnrollmentApprovalRepository enrollmentApprovalRepository)
        {
            _enrollmentApprovalRepository = enrollmentApprovalRepository;
        }

        public async Task<IEnumerable<PendingApprovalDetailsDto>> GetEnrollmentPendingApprovals()
        {
            return await _enrollmentApprovalRepository.GetEnrollmentPendingApprovals();
        }

        public async Task<ApprovalResult> ManageApprovalWorkFlowAsync(int studentId, int courseId, ApprovalCompletionDto dto)
        {
            var enrollment = await _enrollmentApprovalRepository.GetEnrollmentDetailsAsync(studentId, courseId);
            if (enrollment == null)
                return ApprovalResult.NotFound;

            if (enrollment.Status != EnrollmentStatus.Pending)
                return ApprovalResult.InvalidTransition;

            try
            {
                switch (dto.Status)
                {
                    case EnrollmentStatus.Approved:
                        enrollment.Approve();
                        break;

                    case EnrollmentStatus.Rejected:
                        enrollment.Reject(dto.RejectedReason);
                        break;

                    default:
                        return ApprovalResult.InvalidTransition;
                }
            }
            catch (Exception ex)
            {
                return ApprovalResult.InvalidTransition;
            }


            enrollment.Status = dto.Status;
            enrollment.RejectedReason = dto.RejectedReason;

            var affectedRows = await _enrollmentApprovalRepository.ManageApprovalWorkFlowAsync(enrollment);
            if (affectedRows == 0)
                return ApprovalResult.InvalidTransition;

            return dto.Status switch
            {
                EnrollmentStatus.Approved => ApprovalResult.Approved,
                EnrollmentStatus.Rejected => ApprovalResult.Rejected,
                _ => ApprovalResult.InvalidTransition
            };
        }
    }
}
