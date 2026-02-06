using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class EnrollRepository : IEnrollRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public EnrollRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> IsEnrollmentExists(int studentId, int courseId)
        {
            var sql = @"
                    SELECT 1
                    FROM [dbo].[Enrollment]
                    WHERE [StudentId] = @studentId AND 
                          [CourseId] = @courseId AND
                          [IsActive] = 1;
            ";
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryFirstOrDefaultAsync<int>(sql, new { studentId, courseId });
            return result > 0;
        }

        public async Task<int> EnrollStudentAsync(int studentId, int courseId)
        {
            var sql = @"
                INSERT INTO [dbo].[Enrollment](
                    [StudentId],
                    [CourseId],
                    [Status],
                    [RequestedAt]
                ) VALUES (
                    @studentId,
                    @courseId,
                    1,
                    GETDATE()
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(sql, new { studentId, courseId });
        }
    }
}
