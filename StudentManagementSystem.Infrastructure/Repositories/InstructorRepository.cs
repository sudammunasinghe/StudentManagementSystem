using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public InstructorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Instructor?> GetInstructorDetailsByInstructorIdAsync(int instructorId)
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
    }
}
