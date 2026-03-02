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

        public async Task CompleteStudentEnrollmentApprovalAsync(EnrollmentApprovalCompletionDto enrollmentApproval)
        {
            var enrollment =
                await _adminRepository.GetEnrollmentDetailsAsync(enrollmentApproval.StudentId, enrollmentApproval.CourseId);

            if (enrollment == null)
                throw new NotFoundException("Enrollment not found");

            if (enrollment.Status != EnrollmentStatus.Pending)
                throw new BadRequestException("Enrollment is not in pending status");

            if (enrollmentApproval.Status == EnrollmentStatus.Approved)
                enrollment.Approve();
            else if (enrollmentApproval.Status == EnrollmentStatus.Rejected)
                enrollment.Reject(enrollmentApproval.RejectedReason);
            else
                throw new BadRequestException("Invalid approval status");
            await _adminRepository.CompleteStudentEnrollmentApprovalAsync(enrollment);

        }

        public async Task CompleteInstructorRegistrationApprovalAsync(InstructorRegeistrationApprovalCompletionDto instructorApproval)
        {
            var instructor =
                await _adminRepository.GetInstructorDetailsByInstructorIdAsync(instructorApproval.InstructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found");

            if (instructor.Status != EnrollmentStatus.Pending)
                throw new BadRequestException("Instructor registration is not in pending status");

            if (instructorApproval.Status == EnrollmentStatus.Approved)
                instructor.Approve();
            else if (instructorApproval.Status == EnrollmentStatus.Rejected)
                instructor.Reject(instructorApproval.RejectedReason);
            else
                throw new BadRequestException("Invalid approval status");

            await _adminRepository.CompleteInstructorRegistrationApprovalAsync(instructor);
        }
    }
}
