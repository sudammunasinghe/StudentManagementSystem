using Dapper;
using StudentManagementSystem.Application.DTOs.Approval;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class EnrollmentApprovalRepository : IEnrollmentApprovalRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public EnrollmentApprovalRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<PendingApprovalDetailsDto>> GetEnrollmentPendingApprovals()
        {
            var sql = @"
                SELECT
	                STD.[Id] [StudentId],
	                CRS.[Id] [CourseId],
	                CONCAT(STD.[FirstName], ' ',STD.[LastName]) [FullName],
	                CRS.[Title] [CourseName],
	                EAS.[Description] [ApprovalStatus],
	                ENR.[RequestedAt]
                FROM [dbo].[Enrollment] ENR
                	INNER JOIN [dbo].[Student] STD ON ENR.[StudentId] = STD.[Id]
                	INNER JOIN [dbo].[Course] CRS ON ENR.[CourseId] = CRS.[Id]
                    INNER JOIN [dbo].[VW_EnrollmentApprovalStatus] EAS ON ENR.[Status] = EAS.[Id]
                WHERE ENR.[Status] = 1
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<PendingApprovalDetailsDto>(sql);
        }

        public async Task<Enrollment?> GetEnrollmentDetailsAsync(int studentId, int courseId)
        {
            var sql = @"
                SELECT
                    [StudentId],        
                    [CourseId],
                    [Status],
                    [RequestedAt],
                    [ApprovedAt],
                    [RejectedReason],
                    [IsActive],
                    [LastModifiedDateTime]
                FROM [dbo].[Enrollment]
                WHERE [StudentId] = @studentId AND [CourseId] = @CourseId;  
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Enrollment>(sql, new { studentId, courseId });
        }

        public async Task<int> ManageApprovalWorkFlowAsync(Enrollment approvalDetails)
        {
            var sql = @"
                UPDATE [dbo].[Enrollment]
                    SET
                        [Status] = @Status,
                        [ApprovedAt] = GETDATE(),
                        [RejectedReason] = @RejectedReason,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [StudentId] = @StudentId AND [CourseId] = @CourseId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, approvalDetails);
        }
    }
}
