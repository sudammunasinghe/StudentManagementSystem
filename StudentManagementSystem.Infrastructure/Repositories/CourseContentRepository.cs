using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public CourseContentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task UploadCourseContentAsync(CourseContent newContent)
        {
            var sql = @"
                INSERT INTO [dbo].[CourseContent]
                (
                	[CourseId],
                	[InstructorId],
                	[Title],
                	[Description],
                	[ContentType],
                	[FileUrl],
                	[FileSize]
                )
                VALUES(
                    @CourseId,
                    @InstructorId,
                    @Title,
                    @Description,
                    @ContentType,
                    @FileUrl,
                    @FileSize
                );
            ";
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteScalarAsync(sql, newContent);
        }
    }
}
