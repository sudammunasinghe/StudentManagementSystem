using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public StudentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Student?> GetStudentDetailsByStudentIdAsync(int stdId)
        {
            var sql = @"
                SELECT 
                    [Id],
                    [FirstName],
                    [LastName],
                    [Address],
                    [Email],
                    [NIC] 
                FROM [dbo].[Student] 
                WHERE [IsActive] = 1 AND [Id] = @stdId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<Student>(sql, new { stdId });
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            var sql = @"
                SELECT 
                    [Id],
                    [FirstName],
                    [LastName],
                    [Address],
                    [Email],
                    [NIC] 
                FROM [dbo].[Student] 
                WHERE [IsActive] = 1;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryAsync<Student>(sql);
        }

        public async Task<int> CreateStudentAsync(Student newStudent)
        {
            var sql = @"
                INSERT INTO [dbo].[Student](
                    [FirstName],
                    [LastName],
                    [Address],
                    [Email],
                    [NIC]
                ) 
                VALUES
                (
                    @FirstName,
                    @LastName,
                    @Address,
                    @Email,
                    @NIC
                ); 
                SELECT CAST(SCOPE_IDENTITY() as int)";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(sql, newStudent);
        }

        public async Task UpdateStudentDetailsAsync(Student updatedStudent)
        {
            var sql = @"
                UPDATE [dbo].[Student]
                    SET 
                        [FirstName] = @FirstName,
                        [LastName] = @LastName,
                        [Address] = @Address,
                        [Email] = @Email,
                        [NIC] = @NIC,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @Id;
            ";
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(sql, updatedStudent);
        }

        public async Task<int> IncativateStudentByStudentIdAsync(int stdId)
        {
            var sql = @"
                UPDATE [dbo].[Student]
                    SET
                        [IsActive] = 0,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @stdId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.ExecuteAsync(sql, new { stdId });
        }
    }
}
