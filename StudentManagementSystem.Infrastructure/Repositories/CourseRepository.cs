using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public CourseRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Course?> GetCourseDetailsByCourseIdAsync(int courseId)
        {
            var sql = @"
                SELECT
                    [Id],
                    [Title],
                    [Credits]
                FROM [dbo].[Course]
                WHERE [IsActive] = 1 AND [Id] = @courseId;
                    
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Course>(sql, new { courseId });
        }
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            var sql = @"
                SELECT
                    [Id],
                    [Title],
                    [Credits]
                FROM [dbo].[Course]
                WHERE [IsActive] = 1;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Course>(sql);
        }

        public async Task<int> CreateNewCourseAsync(Course newCourse)
        {
            var sql = @"
                INSERT INTO [dbo].[Course]
                (
                    [Title],
                    [Credits]
                )
                VALUES(
                    @Title,
                    @Credits
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(sql, newCourse);
        }

        public async Task<int> UpdateCourseDetailsAsync(Course updatedCourse)
        {
            var sql = @"
                UPDATE [dbo].[Course]
                    SET
                        [Title] = @Title,
                        [Credits] = @Credits,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @Id;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, updatedCourse);
        }

        public async Task<int> InactivateCourseByCourseIdAsync(int courseId)
        {
            var sql = @"
                UPDATE [dbo].[Course]
                    SET
                        [IsActive] = 0,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @courseId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, new { courseId });
        }
    }
}
