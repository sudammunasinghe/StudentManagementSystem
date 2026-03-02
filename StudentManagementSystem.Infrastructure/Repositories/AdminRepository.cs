using Dapper;
using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _queryLoader;

        private readonly string _Select_ApprovalDetails;
        private readonly string _Select_EnrollmentDetails;
        private readonly string _Update_EnrollmentStatus;
        private readonly string _Select_InstructorDetailsByInstructorId;
        private readonly string _Update_InstructorDetailsByInstructorId;
        public AdminRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader queryLoader)
        {
            _connectionFactory = connectionFactory;
            _queryLoader = queryLoader;
            _Select_ApprovalDetails = _queryLoader.Load("Admin", "Select_ApprovalDetails.sql");
            _Select_EnrollmentDetails = _queryLoader.Load("Admin", "Select_EnrollmentDetails.sql");
            _Update_EnrollmentStatus = _queryLoader.Load("Admin", "Update_EnrollmentStatus.sql");
            _Select_InstructorDetailsByInstructorId = _queryLoader.Load("Admin", "Select_InstructorDetailsByInstructorId.sql");
            _Update_InstructorDetailsByInstructorId = _queryLoader.Load("Admin", "Update_InstructorDetailsByInstructorId.sql");
        }

        public async Task<IEnumerable<ApprovalDetailsDto>> GetAllApprovalDetailsAsync(int? statusCode)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<ApprovalDetailsDto>(_Select_ApprovalDetails, new { StatusCode = statusCode });
        }

        public async Task<Enrollment?> GetEnrollmentDetailsAsync(int studentId, int courseId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Enrollment>(_Select_EnrollmentDetails, new { StudentId = studentId, CourseId = courseId });
        }

        public async Task CompleteStudentEnrollmentApprovalAsync(Enrollment updatedEnrollment)
        {
            using var db = _connectionFactory.CreateConnection();
            var parameters = new
            {
                StudentId = updatedEnrollment.StudentId,
                CourseId = updatedEnrollment.CourseId,
                Status = updatedEnrollment.Status,
                RejectedReason = updatedEnrollment.RejectedReason
            };
            await db.ExecuteAsync(_Update_EnrollmentStatus, parameters);
        }

        public async Task<Instructor?> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Instructor>(_Select_InstructorDetailsByInstructorId, new { InstructorId = instructorId });
        }

        public async Task CompleteInstructorRegistrationApprovalAsync(Instructor updatedInstructor)
        {
            using var db = _connectionFactory.CreateConnection();
            var parameters = new
            {
                InstructorId = updatedInstructor.Id,
                RejectedReason = updatedInstructor.RejectedReason,
                Status = updatedInstructor.Status
            };
            await db.ExecuteAsync(_Update_InstructorDetailsByInstructorId, parameters);
        }
    }
}
