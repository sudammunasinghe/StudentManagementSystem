using Dapper;
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
                        [RegistrationNumber],
                        [FirstName], 
	                    [LastName], 
                        [ContactNumber],
	                    [Address], 
	                    [NIC], 
	                    [DateOfBirth],
	                    [Gender],
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
                        [RegistrationNumber],
                        [FirstName], 
	                    [LastName], 
                        [ContactNumber],
	                    [Address], 
	                    [NIC], 
	                    [DateOfBirth],
	                    [Gender],
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
	                    [FirstName], 
	                    [LastName], 
                        [ContactNumber],
	                    [Address], 
	                    [NIC], 
	                    [DateOfBirth],
	                    [Gender],
                    	[Email],
                    	[PasswordHash],
                        [RoleId]
                    )
                    VALUES(
                        @FirstName,
                        @LastName,
                        @ContactNumber,
                        @Address,
                        @NIC,
                        @DateOfBirth,
                        @Gender,
                    	@Email,
                        @PasswordHash,
                        @RoleId
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var userId = await db.ExecuteScalarAsync<int>(
                    userSql,
                    new
                    {
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        ContactNumber = newUser.ContactNumber,
                        Address = newUser.Address,
                        NIC = newUser.NIC,
                        DateOfBirth = newUser.DateOfBirth,
                        Gender = newUser.Gender,
                        Email = newUser.Email,
                        PasswordHash = newUser.PasswordHash,
                        RoleId = newUser.RoleId
                    },
                    transaction
                );

                var studentSql = @"
                    INSERT INTO [dbo].[Student](
                        [UserId],
                        [GPA]
                    ) 
                    VALUES
                    (
                        @UserId,
                        @GPA
                    ); 
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var studentId = await db.ExecuteScalarAsync<int>(
                    studentSql,
                    new { UserId = userId, GPA = studentDetails.GPA },
                    transaction
                );

                //Generate registration number and update the user table
                var registrationNumber = $"STD/{DateTime.UtcNow.Year}/{studentId:D4}";
                var updateRegistrationNumberSql = @"
                    UPDATE [dbo].[User]
                    SET
                        [RegistrationNumber] = @RegistrationNumber,
                        [LastModifiedDateTime] = GETDATE()
                    WHERE [Id] = @UserId;
                ";
                await db.ExecuteAsync(updateRegistrationNumberSql, new { UserId = userId, RegistrationNumber = registrationNumber }, transaction);

                if (studentDetails.EducationDetails != null && studentDetails.EducationDetails.Any())
                {
                    var educationSql = @"
                        INSERT INTO [dbo].[Education]
                        (
                        	[StudentId],
                        	[Institute],
                        	[Degree],
                        	[Major],
                        	[StartingDate],
                        	[EndingDate],
                        	[IsStudying],
                        	[Description]
                        )
                        VALUES(
                        	@StudentId,
                        	@Institute,
                        	@Degree,
                        	@Major,
                        	@StartingDate,
                        	@EndingDate,
                        	@IsStudying,
                        	@Description
                        )
                    ";

                    foreach (var edu in studentDetails.EducationDetails)
                    {
                        await db.ExecuteAsync(
                            educationSql,
                            new
                            {
                                StudentId = studentId,
                                Institute = edu.Institute,
                                Degree = edu.Degree,
                                Major = edu.Major,
                                StartingDate = edu.StartingDate,
                                EndingDate = edu.EndingDate,
                                IsStudying = edu.IsStudying,
                                Description = edu.Description
                            },
                            transaction
                        );
                    }
                }
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
                    	[FirstName], 
	                    [LastName], 
                        [ContactNumber],
	                    [Address], 
	                    [NIC], 
	                    [DateOfBirth],
	                    [Gender],
                    	[Email],
                    	[PasswordHash],
                        [RoleId]
                    )
                    VALUES
                    (
                    	@FirstName,
                        @LastName,
                        @ContactNumber,
                        @Address,
                        @NIC,
                        @DateOfBirth,
                        @Gender,
                    	@Email,
                        @PasswordHash,
                        @RoleId
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";
                var userId = await db.ExecuteScalarAsync<int>(
                    userSql,
                    new
                    {
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        ContactNumber = newUser.ContactNumber,
                        Address = newUser.Address,
                        NIC = newUser.NIC,
                        DateOfBirth = newUser.DateOfBirth,
                        Gender = newUser.Gender,
                        Email = newUser.Email,
                        PasswordHash = newUser.PasswordHash,
                        RoleId = newUser.RoleId
                    },
                    transaction
                );

                var instructorSql = @"
                    INSERT INTO [dbo].[Instructor](
                        [UserId],
                        [ExperienceYears],
                        [PreferredSalary]

                    ) 
                    VALUES 
                    (
                        @UserId,
                        @ExperienceYears,
                        @PreferredSalary
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);
                ";

                var instructorId = await db.ExecuteScalarAsync<int>(
                    instructorSql,
                    new { UserId = userId, ExperienceYears = instructorDetails.ExperienceYears, PreferredSalary = instructorDetails.PreferredSalary },
                    transaction
                );

                //Generate registration number and update the user table
                var registrationNumber = $"INS/{DateTime.UtcNow.Year}/{instructorId:D4}";
                var updateRegistrationNumberSql = @"
                    UPDATE [dbo].[User]
                        SET
                            [RegistrationNumber] = @RegistrationNumber,
                            [LastModifiedDateTime] = GETDATE()
                        WHERE [Id] = @UserId;
                ";
                await db.ExecuteAsync(updateRegistrationNumberSql, new { UserId  = userId, RegistrationNumber = registrationNumber }, transaction );

                if (instructorDetails.InstructorExperiences != null && instructorDetails.InstructorExperiences.Any())
                {
                    var experienceDetailsSql = @"
                        INSERT INTO [dbo].[InstructorExperience]
                        (
                        	[InstructorId],
                        	[CompanyName],
                        	[JobTitle],
                        	[EmployementType],
                        	[Location],
                        	[StartDate],
                        	[EndDate],
                        	[IsCurrentlyWorking],
                        	[Description]
                        )
                        VALUES
                        (
                        	@InstructorId,
                        	@CompanyName,
                        	@JobTitle,
                        	@EmployementType,
                        	@Location,
                        	@StartDate,
                        	@EndDate,
                        	@IsCurrentlyWorking,
                        	@Description
                        )
                    ";

                    foreach (var exp in instructorDetails.InstructorExperiences)
                    {
                        await db.ExecuteAsync(
                            experienceDetailsSql,
                            new
                            {
                                InstructorId = instructorId,
                                CompanyName = exp.CompanyName,
                                JobTitle = exp.JobTitle,
                                EmployementType = exp.EmployementType,
                                Location = exp.Location,
                                StartDate = exp.StartDate,
                                EndDate = exp.EndDate,
                                IsCurrentlyWorking = exp.IsCurrentlyWorking,
                                Description = exp.Description
                            },
                            transaction
                        );
                    }
                }
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
                    [FirstName], 
	                [LastName], 
                    [ContactNumber],
	                [Address], 
	                [NIC], 
	                [DateOfBirth],
	                [Gender],
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
            await db.ExecuteAsync(sql, new { UserId = userId, NewPasswordHash = newPasswordHash });
        }
    }
}
