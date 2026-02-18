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

        public async Task<CourseContent?> GetCourseContentByCourseContentIdAsync(int? contentId)
        {
            var sql = @"
                SELECT
                    [Id],
                    [CourseId],
                    [InstructorId],
                    [Title],
                    [Description],
                    [ContentType],
                    [FileUrl],
                    [FileSize]
                FROM [dbo].[CourseContent]
                WHERE [Id] = @contentId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<CourseContent>(sql, new { contentId });
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

        public async Task<int> UpdateCourseMetaDataAsync(CourseContent updatedContent)
        {
            var sql = @"
                UPDATE [dbo].[CourseContent]
                SET
                    [CourseId] = @CourseId,
                	[InstructorId] = @InstructorId,
                	[Title] = @Title,
                	[Description] = @Description,
                	[ContentType] = @ContentType,
                	[FileUrl] = @FileUrl,
                	[FileSize] = @FileSize,
                    [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @Id;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, updatedContent);
        }

        public async Task<int> InactivateCourseContentByCourseContentIdAsync(int? contentId)
        {
            var sql = @"
                UPDATE [dbo].[CourseContent]
                SET
                    [IsActive] = 0,
                    [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @contentId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, new { contentId });
        }
    }
}
