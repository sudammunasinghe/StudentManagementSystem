using Dapper;
using StudentManagementSystem.Application.DTOs.Course;
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

        public async Task<Course?> GetCourseDetailsByCourseIdAsync(int? courseId)
        {
            var sql = @"
                SELECT
                    [Id],
                    [Title],
                    [Credits],
                    [InstructorId]
                FROM [dbo].[Course]
                WHERE [IsActive] = 1 AND [Id] = @courseId;
                    
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Course>(sql, new { courseId });
        }
        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var sql = @"
                SELECT 
                	CRS.[Id],
                	CRS.[Title],
                	CRS.[Credits],
                	COUNT(ENR.[StudentId]) [NoOfEnrolledStudents]
                FROM [dbo].[Course] CRS
                	LEFT JOIN [dbo].[Enrollment] ENR ON CRS.[Id] = ENR.[CourseId]
                WHERE ENR.[IsActive] = 1
                GROUP BY CRS.[Id],CRS.[Title],CRS.[Credits];
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<CourseDto>(sql);
        }

        public async Task<int> CreateNewCourseAsync(Course newCourse)
        {
            var sql = @"
                INSERT INTO [dbo].[Course]
                (
                    [Title],
                    [Credits],
                    [InstructorId]
                )
                VALUES(
                    @Title,
                    @Credits,
                    @InstructorId
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
                        [InstructorId] = @InstructorId,
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

        public async Task<int> GetEnrolledStudentCountForCourseByCourseIdAsync(int courseId)
        {
            var sql = @"
                SELECT
	                STD.[FirstName],
	                STD.[LastName],
	                STD.[Address],
	                STD.[Email],
	                STD.[NIC]
                FROM [dbo].[Enrollment] ENR
                	INNER JOIN [dbo].[Student] STD ON ENR.[StudentId] = STD.[Id]
                WHERE ENR.[IsActive] = 1 AND ENR.[CourseId] = @courseId;
            ";
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<Student>(sql, new { courseId });
            return result.Count();
        }
    }
}
