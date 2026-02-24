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

        public async Task UpdateProfileDetailsAsync(ProfileEntity profile, string role)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                //Update User Data
                var userSql = @"
                    UPDATE [dbo].[User]
                        SET
                        	[FirstName] = @FirstName,
                        	[LastName] = @LastName,
                        	[Address] = @Address,
                        	[ContactNumber] = @ContactNumber,
                        	[Email] = @Email,
                        	[NIC] = @NIC,
                        	[LastModifiedDateTime] = GETDATE()
                        WHERE [Id] = @Id;
                ";
                await db.ExecuteAsync(userSql, profile, transaction );

                if(role == "Student")
                {
                    var studentId = profile.EducationalDetails?.FirstOrDefault()?.StudentId;
                    var existingIdsSql = @"
                        SELECT
                            [Id]
                        FROM [dbo].[Education]
                        WHERE [StudentId] = @StudentId;
                    ";
                    var existingIds = (await db.QueryAsync<int>( existingIdsSql, new { StudentId = studentId }, transaction )).ToList();
                    var incomingIds = profile?.EducationalDetails?
                        .Where(edu => edu.Id.HasValue)
                        .Select(edu => edu.Id.Value)
                        .ToList() ?? new List<int>();

                    var IdsToInactivate = existingIds.Except(incomingIds).ToList();

                    //Inactivate Education records
                    if (IdsToInactivate.Any())
                    {
                        var inactivateRecordsSql = @"
                            UPDATE [dbo].[Education]
                                SET
                                    [IsActive] = 0,
                                    [LastModifiedDateTime] = GETDATE()
                                WHERE [Id] IN @Ids;
                            ";
                        await db.ExecuteAsync(inactivateRecordsSql, new { Ids = IdsToInactivate }, transaction );
                    }

                    //Insert or Update Educational Details
                    foreach(var edu in profile.EducationalDetails)
                    {
                        if (edu.Id.HasValue)
                        {
                            var updateEducation = @"
                                UPDATE [dbo].[Education]
                                SET
                                	[Institute] = @Institute,
                                    [Degree] = @Degree,
                                    [Major] = @Major,
                                    [StartingDate] = @StartingDate,
                                    [EndingDate] = @EndingDate,
                                    [IsStudying] = @IsStudying,
                                    [Description] = @Description,
                                	[LastModifiedDateTime] = GETDATE()
                                WHERE [Id] = @Id;
                            ";
                            await db.ExecuteAsync(
                                updateEducation,
                                new
                                {
                                    Id = edu.Id,
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
                        else
                        {
                            var insertEducationSql = @"
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
                            await db.ExecuteAsync(
                                insertEducationSql,
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


                }
                else if(role == "Instructor")
                {
                    var instructorId = profile.InstructorExperiences?.FirstOrDefault()?.InstructorId;

                    var existingIdsSql = @"
                        SELECT 
                            [Id] 
                        FROM [dbo].[InstructorExperience] 
                        WHERE [InstructorId] = @InstructorId";

                    var existingIds = (await db.QueryAsync<int>(existingIdsSql, new { InstructorId = instructorId }, transaction)).ToList();
                    var incomingIds = profile.InstructorExperiences?
                        .Where(exp => exp.Id.HasValue)
                        .Select(exp => exp.Id.Value)
                        .ToList() ?? new List<int>();
                    var IdsToInactivate = existingIds.Except(incomingIds).ToList();

                    //Inactivate Experience record
                    if (IdsToInactivate.Any())
                    {
                        var inactivateRecordsSql = @"
                        UPDATE [dbo].[InstructorExperience]
                            SET
                                [IsActive] = 0,
                                [LastModifiedDateTime] = GETDATE()
                            WHERE [Id] IN @Ids;
                        ";
                        await db.ExecuteAsync(inactivateRecordsSql, new { Ids = IdsToInactivate }, transaction);
                    }

                    //Insert or Update experince records
                    foreach(var exp in profile.InstructorExperiences)
                    {
                        if (exp.Id.HasValue)
                        {
                            var updateExperienceSql = @"
                                UPDATE [dbo].[InstructorExperience]
                                SET
                                	[CompanyName] = @CompanyName,
                                	[JobTitle] = @JobTitle,
                                	[EmployementType] = @EmployementType,
                                	[Location] = @Location,
                                	[StartDate] = @StartDate,
                                	[EndDate] = @EndDate,
                                	[IsCurrentlyWorking] = @IsCurrentlyWorking,
                                	[Description] = @Description
                                WHERE [Id] = @Id;
                            ";
                            await db.ExecuteAsync(
                                updateExperienceSql,
                                new
                                {
                                    Id = exp.Id,
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
                        else
                        {
                            var insertExperienceSql = @"
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
                            await db.ExecuteAsync(
                                insertExperienceSql,
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

                }
                transaction.Commit();

            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
