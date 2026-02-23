using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public AccountRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Education>> GetEducationalDetailsByUserIdAsync(int userId)
        {
            var sql = @"
                SELECT
	                EDC.[Id],
	                EDC.[StudentId],
	                EDC.[Institute],
	                EDC.[Degree],
	                EDC.[Major],
	                EDC.[StartingDate],
	                EDC.[EndingDate],
	                EDC.[IsStudying],
	                EDC.[Description],
	                EDC.[IsActive],
	                EDC.[CreatedDateTime],
	                EDC.[LastModifiedDateTime]
                FROM [dbo].[User] US
                	INNER JOIN [dbo].[Student] STD ON US.[Id] = STD.[UserId] AND STD.[IsActive] = 1
                	INNER JOIN [dbo].[Education] EDC ON STD.[Id] = EDC.[StudentId] AND EDC.[IsActive] = 1
                WHERE US.[Id] = @UserId;
            ";
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<Education>(sql, new { UserId = userId });
            return result.ToList();
        }

        public async Task<List<InstructorExperience>> GetInstructorExperienceDetailsByUserIdAsync(int userId)
        {
            var sql = @"
                SELECT
	                IE.[Id],
	                IE.[InstructorId],
	                IE.[CompanyName],
	                IE.[JobTitle],
	                IE.[EmployementType],
	                IE.[Location],
	                IE.[StartDate],
	                IE.[EndDate],
	                IE.[IsCurrentlyWorking],
	                IE.[Description],
	                IE.[IsActive],
	                IE.[CreatedDateTime],
	                IE.[LastModifiedDateTime]
                FROM [dbo].[User] US
                	INNER JOIN [dbo].[Instructor] INS ON US.[Id] = INS.[UserId] AND INS.[IsActive] = 1
                	INNER JOIN [dbo].[InstructorExperience] IE ON INS.[Id] = IE.[InstructorId] AND IE.[IsActive] = 1
                WHERE US.[Id] = @UserId
            ";
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<InstructorExperience>(sql, new { UserId = userId });
            return result.ToList();
        }
    }
}
