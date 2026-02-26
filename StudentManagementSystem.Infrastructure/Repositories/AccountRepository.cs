using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _sqlQueryLoader;

        private readonly string _Select_EducationalDetails;
        private readonly string _Select_InstructorExperienceDetails;
        private readonly string _UpdateUser;
        private readonly string _Select_ExistingEducationIds;
        private readonly string _UpdateEducation;
        private readonly string _UpdateEducationDetails;
        private readonly string _InsertEducation;
        private readonly string _SelectExistingInstructorExperienceIds;
        private readonly string _UpdateInstructorExperience;
        private readonly string _UpdateInstructorExperienceDetails;
        private readonly string _InsertInstructorExperience;
        public AccountRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader sqlQueryLoader)
        {
            _connectionFactory = connectionFactory;
            _sqlQueryLoader = sqlQueryLoader;
            _Select_EducationalDetails = _sqlQueryLoader.Load("Account", "Select_EducationalDetails.sql");
            _Select_InstructorExperienceDetails = sqlQueryLoader.Load("Account", "Select_InstructorExperienceDetails.sql");
            _UpdateUser = _sqlQueryLoader.Load("Account", "UpdateUser.sql");
            _Select_ExistingEducationIds = sqlQueryLoader.Load("Account", "Select_ExistingEducationIds.sql");
            _UpdateEducation = sqlQueryLoader.Load("Account", "UpdateEducation.sql");
            _UpdateEducationDetails = sqlQueryLoader.Load("Account", "UpdateEducationDetails.sql");
            _InsertEducation = sqlQueryLoader.Load("Account", "InsertEducation.sql");
            _SelectExistingInstructorExperienceIds = sqlQueryLoader.Load("Account", "SelectExistingInstructorExperienceIds.sql");
            _UpdateInstructorExperience = sqlQueryLoader.Load("Account", "UpdateInstructorExperience.sql");
            _UpdateInstructorExperienceDetails = sqlQueryLoader.Load("Account", "UpdateInstructorExperienceDetails.sql");
            _InsertInstructorExperience = sqlQueryLoader.Load("Account", "InsertInstructorExperience.sql");
        }

        public async Task<List<Education>> GetEducationalDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<Education>(_Select_EducationalDetails, new { UserId = userId });
            return result.ToList();
        }

        public async Task<List<InstructorExperience>> GetInstructorExperienceDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<InstructorExperience>(_Select_InstructorExperienceDetails, new { UserId = userId });
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
                await db.ExecuteAsync(_UpdateUser, profile, transaction );

                if(role == nameof(Roles.Student))
                {
                    var studentId = int.Parse(profile.RegistrationNumber.Substring(profile.RegistrationNumber.Length - 4));
                    var existingIds = (await db.QueryAsync<int>(_Select_ExistingEducationIds, new { StudentId = studentId }, transaction )).ToList();
                    var incomingIds = profile?.EducationalDetails?
                        .Where(edu => edu.Id.HasValue)
                        .Select(edu => edu.Id.Value)
                        .ToList() ?? new List<int>();

                    var IdsToInactivate = existingIds.Except(incomingIds).ToList();

                    //Inactivate Education records
                    if (IdsToInactivate.Any())
                        await db.ExecuteAsync(_UpdateEducation, new { Ids = IdsToInactivate }, transaction );
                    
                    //Insert or Update Educational Details
                    if (profile.EducationalDetails != null)
                    {
                        foreach (var edu in profile.EducationalDetails)
                        {
                            if (edu.Id.HasValue)
                            {
                                await db.ExecuteAsync(
                                    _UpdateEducationDetails,
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
                                await db.ExecuteAsync(
                                    _InsertEducation,
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

                }
                else if(role == nameof(Roles.Instructor))
                {
                    var instructorId = int.Parse(profile.RegistrationNumber.Substring(profile.RegistrationNumber.Length - 4));
                    var existingIds = (await db.QueryAsync<int>(_SelectExistingInstructorExperienceIds, new { InstructorId = instructorId }, transaction )).ToList();
                    var incomingIds = profile.InstructorExperiences?
                        .Where(exp => exp.Id.HasValue)
                        .Select(exp => exp.Id.Value)
                        .ToList() ?? new List<int>();
                    var IdsToInactivate = existingIds.Except(incomingIds).ToList();

                    //Inactivate Experience record
                    if (IdsToInactivate.Any())
                        await db.ExecuteAsync(_UpdateInstructorExperience, new { Ids = IdsToInactivate }, transaction);

                    //Insert or Update experince records
                    if (profile.InstructorExperiences != null)
                    {
                        foreach (var exp in profile.InstructorExperiences)
                        {
                            if (exp.Id.HasValue)
                            {
                                await db.ExecuteAsync(
                                    _UpdateInstructorExperienceDetails,
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
                                await db.ExecuteAsync(
                                    _InsertInstructorExperience,
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
