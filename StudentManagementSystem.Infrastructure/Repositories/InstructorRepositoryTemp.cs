using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class InstructorRepositoryTemp : IInstructorRepositoryTemp
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public InstructorRepositoryTemp(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Instructor?> GetInstructorDetailsByInstructorIdAsync(int? instructorId)
        {
            var sql = @"
                SELECT
                    [Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [NIC],
                    [Address]
                FROM [dbo].[Instructor]
                WHERE [IsActive] = 1 AND [Id] = @InstructorId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Instructor>(sql, new { InstructorId = instructorId });
        }

        public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
        {
            var sql = @"
                SELECT
                    [Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [NIC],
                    [Address]
                FROM [dbo].[Instructor]
                WHERE [IsActive] = 1;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Instructor>(sql);
        }

        public async Task<int> CreateInstructorAsync(Instructor newInstructor)
        {
            var sql = @"
                INSERT INTO [dbo].[Instructor](
                    [FirstName],
                    [LastName],
                    [Email],
                    [NIC],
                    [Address]
                ) VALUES (
                    @FirstName,
                    @LastName,
                    @Email,
                    @NIC,
                    @Address
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(sql, newInstructor);
        }

        public async Task<int> UpdateInstructorDetailsAsync(Instructor updatedInstructor)
        {
            var sql = @"
                UPDATE [dbo].[Instructor]
                    SET
                        [FirstName] = @FirstName,
                        [LastName] = @LastName,
                        [Email] = @Email,
                        [NIC] = @NIC,
                        [Address] = @Address,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @Id;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, updatedInstructor);
        }

        public async Task<int> InactivateInstructorByInstructorIdAsync(int instructorId)
        {
            var sql = @"
                UPDATE [dbo].[Instructor]
                    SET
                        [IsActive] = 0,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @InstructorId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, new { InstructorId = instructorId });
        }

        public async Task<IEnumerable<Course>> GetOwnCoursesByInstructorIdAsync(int instructorId)
        {
            var sql = @"
                SELECT
                    [Id],
                    [Title],
                    [Credits],
                    [InstructorId]
                FROM [dbo].[Course]
                WHERE [IsActive] = 1 AND [InstructorId] = @instructorId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Course>(sql, new { instructorId });
        }

        public async Task<IEnumerable<CourseContent>> GetCourseContentByInstructorIdAsync(int instructorId)
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
                WHERE [IsActive] = 1 AND [InstructorId] = @instructorId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<CourseContent>(sql, new { instructorId });
        }
    }
}
