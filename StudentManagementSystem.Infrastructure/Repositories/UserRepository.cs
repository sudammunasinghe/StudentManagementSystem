using Dapper;
using Newtonsoft.Json.Linq;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = @"
                    SELECT
                        [Id],
	                    [Email],
	                    [PasswordHash],
                        [RoleId]
                    FROM [dbo].[User]
                    WHERE [IsActive] = 1 AND [Email] = @Email;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var sql = @"
                 SELECT
                        [Id],
	                    [Email],
	                    [PasswordHash],
                        [RoleId]
                    FROM [dbo].[User]
                    WHERE [IsActive] = 1 AND [Id] = @UserId;
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User?>(sql, new { UserId = userId });
        }

        public async Task<int> CreateNewStudentUserAsync(User newUser, Student studentDetails)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();

            using var transaction = db.BeginTransaction();
            try
            {
                var userSql = @"
                    INSERT  INTO [dbo].[User]
                    (
                    	[Email],
                    	[PasswordHash],
                        [RoleId]
                    )
                    VALUES(
                    	@Email,
                        @PasswordHash,
                        @RoleId
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var userId = await db.QuerySingleAsync<int>(
                    userSql,
                    new { Email = newUser.Email, PasswordHash = newUser.PasswordHash, RoleId = newUser.RoleId },
                    transaction
                );

                var studentSql = @"
                    INSERT INTO [dbo].[Student](
                        [UserId],
                        [FirstName],
                        [LastName],
                        [Address],
                        [NIC],
                        [DateOfBirth],
                        [Gender],
                        [GPA]
                    ) 
                    VALUES
                    (
                        @UserId,
                        @FirstName,
                        @LastName,
                        @Address,
                        @NIC,
                        @DateOfBirth,
                        @Gender,
                        @GPA
                    ); 
                ";

                await db.ExecuteAsync(
                    studentSql,
                    new
                    {
                        UserId = userId,
                        FirstName = studentDetails.FirstName,
                        LastName = studentDetails.LastName,
                        Address = studentDetails.Address,
                        NIC = studentDetails.NIC,
                        GPA = studentDetails.GPA,
                        DateOfBirth = studentDetails.DateOfBirth,
                        Gender = studentDetails.Gender
                    },
                    transaction
                );

                transaction.Commit();
                return userId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<int> CreateNewInstructorUserAsync(User newUser, Instructor instructorDetails)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();

            using var transaction = db.BeginTransaction();
            try
            {
                var userSql = @"
                    INSERT INTO [dbo].[User]
                    (
                    	[Email],
                    	[PasswordHash],
                        [RoleId]
                    )
                    VALUES(
                    	@Email,
                        @PasswordHash,
                        @RoleId
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";
                var userId = await db.QuerySingleAsync<int>(
                    userSql,
                    new { Email = newUser.Email, PasswordHash = newUser.PasswordHash, RoleId = newUser.RoleId },
                    transaction
                );

                var instructorSql = @"
                    INSERT INTO [dbo].[Instructor](
                        [UserId],
                        [FirstName],
                        [LastName],
                        [Address],
                        [NIC],
                        [DateOfBirth],
                        [Gender],
                        [ExperienceYears],
                        [PreferredSalary]

                    ) VALUES (
                        @UserId,
                        @FirstName,
                        @LastName,
                        @Address,
                        @NIC,
                        @DateOfBirth,
                        @Gender,
                        @ExperienceYears,
                        @PreferredSalary
                    );
                ";

                await db.ExecuteAsync(
                    instructorSql,
                    new
                    {
                        UserId = userId,
                        FirstName = instructorDetails.FirstName,
                        LastName = instructorDetails.LastName,
                        NIC = instructorDetails.NIC,
                        Address = instructorDetails.Address,
                        ExperienceYears = instructorDetails.ExperienceYears,
                        PreferredSalary = instructorDetails.PreferredSalary,
                        DateOfBirth = instructorDetails.DateOfBirth,
                        Gender = instructorDetails.Gender
                    },
                    transaction
                );
                transaction.Commit();
                return userId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task SavePasswordResetTokenAsync(int userId, string token, DateTime expiry)
        {
            var sql = @"
                UPDATE [dbo].[User]
                    SET
                        [PasswrodResetToken] = @Token,
                        [PasswrodResetTokenExpiry] = @Expiry,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @UserId;
            ";
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(sql, new { UserId = userId, Token = token, Expiry = expiry });
        }

        public async Task<User?> GetUserByResetTokenAsync(string token)
        {
            var sql = @"
                SELECT
                    [Id],
	                [Email],
	                [PasswordHash],
                    [RoleId],
                    [PasswrodResetToken],
                    [PasswrodResetTokenExpiry]
                FROM [dbo].[User]
                WHERE [IsActive] = 1 AND [PasswrodResetToken] = @Token;
                    
            ";
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Token = token });
        }

        public async Task UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            var sql = @"
                UPDATE [dbo].[User]
                    SET
                        [PasswordHash] = @NewPasswordHash,
                        [PasswrodResetToken] = NULL,
                        [PasswrodResetTokenExpiry] = NULL,
                        [LastModifiedDateTime] = GETDATE()
                WHERE [Id] = @UserId
            ";
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(sql, new { UserId  = userId, NewPasswordHash  = newPasswordHash });
        }
    }
}
