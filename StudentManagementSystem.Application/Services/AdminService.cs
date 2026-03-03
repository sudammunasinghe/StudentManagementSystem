using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<ApprovalDetailsDto>> GetApprovalDetailsAsync(EnrollmentStatus? status)
        {
            if (status.HasValue && !Enum.IsDefined(typeof(EnrollmentStatus), status.Value))
            {
                throw new BadRequestException("Invalid status value");
            }

            int? statusCode = status.HasValue ? (int)status.Value : null;
            return await _adminRepository.GetAllApprovalDetailsAsync(statusCode);
        }

        public async Task<ApprovalResult> CompleteStudentEnrollmentApprovalAsync(EnrollmentApprovalCompletionDto enrollmentApproval)
        {
            var enrollment =
                await _adminRepository.GetEnrollmentDetailsAsync(enrollmentApproval.StudentId, enrollmentApproval.CourseId);

            if (enrollment == null)
                return ApprovalResult.NotFound;

            if (enrollment.Status != EnrollmentStatus.Pending)
                return ApprovalResult.InvalidTransition;

            switch (enrollmentApproval.Status)
            {
                case EnrollmentStatus.Approved:
                    enrollment.Approve();
                    break;
                case EnrollmentStatus.Rejected:
                    enrollment.Reject(enrollmentApproval.RejectedReason);
                    break;
                default:
                    return ApprovalResult.InvalidTransition;
            }

            var affectedRows =
                await _adminRepository.CompleteStudentEnrollmentApprovalAsync(enrollment);

            if (affectedRows == 0)
                return ApprovalResult.InvalidTransition;

            return enrollmentApproval.Status switch
            {
                EnrollmentStatus.Approved => ApprovalResult.Approved,
                EnrollmentStatus.Rejected => ApprovalResult.Rejected,
                _ => ApprovalResult.InvalidTransition
            };
        }

        public async Task<ApprovalResult> CompleteInstructorRegistrationApprovalAsync(InstructorRegeistrationApprovalCompletionDto instructorApproval)
        {
            var instructor =
                await _adminRepository.GetInstructorDetailsByInstructorIdAsync(instructorApproval.InstructorId);

            if (instructor == null)
                return ApprovalResult.NotFound;

            if (instructor.Status != EnrollmentStatus.Pending)
                return ApprovalResult.InvalidTransition;

            switch (instructorApproval.Status)
            {
                case EnrollmentStatus.Approved:
                    instructor.Approve();
                    break;
                case EnrollmentStatus.Rejected:
                    instructor.Reject(instructorApproval.RejectedReason);
                    break;
                default:
                    return ApprovalResult.InvalidTransition;
            }

            var affectedRows =
                await _adminRepository.CompleteInstructorRegistrationApprovalAsync(instructor);

            if (affectedRows == 0)
                return ApprovalResult.InvalidTransition;

            return instructorApproval.Status switch
            {
                EnrollmentStatus.Approved => ApprovalResult.Approved,
                EnrollmentStatus.Rejected => ApprovalResult.Rejected,
                _ => ApprovalResult.InvalidTransition
            };
        }
    }
}
