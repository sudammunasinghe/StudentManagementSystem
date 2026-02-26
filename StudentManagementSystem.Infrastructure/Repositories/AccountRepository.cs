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
        private readonly string _Update_User;
        private readonly string _Select_ExistingEducationIds;
        private readonly string _Update_Education;
        private readonly string _Update_EducationDetails;
        private readonly string _Insert_Education;
        private readonly string _Select_ExistingInstructorExperienceIds;
        private readonly string _Update_InstructorExperience;
        private readonly string _Update_InstructorExperienceDetails;
        private readonly string _Insert_InstructorExperience;
        public AccountRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader sqlQueryLoader)
        {
            _connectionFactory = connectionFactory;
            _sqlQueryLoader = sqlQueryLoader;
            _Select_EducationalDetails = _sqlQueryLoader.Load("Account", "Select_EducationalDetails.sql");
            _Select_InstructorExperienceDetails = _sqlQueryLoader.Load("Account", "Select_InstructorExperienceDetails.sql");
            _Update_User = _sqlQueryLoader.Load("Account", "Update_User.sql");
            _Select_ExistingEducationIds = _sqlQueryLoader.Load("Account", "Select_ExistingEducationIds.sql");
            _Update_Education = _sqlQueryLoader.Load("Account", "Update_Education.sql");
            _Update_EducationDetails = _sqlQueryLoader.Load("Account", "Update_EducationDetails.sql");
            _Insert_Education = _sqlQueryLoader.Load("Account", "Insert_Education.sql");
            _Select_ExistingInstructorExperienceIds = _sqlQueryLoader.Load("Account", "Select_ExistingInstructorExperienceIds.sql");
            _Update_InstructorExperience = _sqlQueryLoader.Load("Account", "Update_InstructorExperience.sql");
            _Update_InstructorExperienceDetails = _sqlQueryLoader.Load("Account", "Update_InstructorExperienceDetails.sql");
            _Insert_InstructorExperience = _sqlQueryLoader.Load("Account", "Insert_InstructorExperience.sql");
        }


        /// <summary>
        /// Retrieves all educational details associated with the specific user
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of <see cref="Education"/>records linked to the specific user.
        /// Returns empty list, if no records are found.
        /// </returns>
        public async Task<List<Education>> GetEducationalDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<Education>(_Select_EducationalDetails, new { UserId = userId });
            return result.ToList();
        }

        /// <summary>
        /// Retrieves all experience details associated with the specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of <see cref="InstructorExperience"/>records linked to the specific user.
        /// Returns empty list, if no records are found.
        /// </returns>
        public async Task<List<InstructorExperience>> GetInstructorExperienceDetailsByUserIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<InstructorExperience>(_Select_InstructorExperienceDetails, new { UserId = userId });
            return result.ToList();
        }


        /// <summary>
        /// Updates the profile details of a user based on their role.
        /// </summary>
        /// <param name="profile">The profile entity containing updated profile details.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns></returns>
        public async Task UpdateProfileDetailsAsync(ProfileEntity profile, string role)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();
            using var transaction = db.BeginTransaction();

            try
            {
                //Updates User details
                await db.ExecuteAsync(_Update_User, profile, transaction );

                if(role == nameof(Roles.Student))
                {
                    //Extract student id from the registration number.
                    var studentId = 
                        int.Parse(profile.RegistrationNumber.Substring(profile.RegistrationNumber.Length - 4));

                    var existingIds = 
                        (await db.QueryAsync<int>(_Select_ExistingEducationIds, new { StudentId = studentId }, transaction )).ToList();

                    var incomingIds = profile?.EducationalDetails?
                        .Where(edu => edu.Id.HasValue)
                        .Select(edu => edu.Id.Value)
                        .ToList() ?? new List<int>();

                    var IdsToInactivate = existingIds.Except(incomingIds).ToList();

                    //Inactivate Education records
                    if (IdsToInactivate.Any())
                        await db.ExecuteAsync(_Update_Education, new { Ids = IdsToInactivate }, transaction );
                    
                    //Insert or Update Educational Details
                    if (profile.EducationalDetails != null)
                    {
                        foreach (var edu in profile.EducationalDetails)
                        {
                            if (edu.Id.HasValue)
                            {
                                await db.ExecuteAsync(
                                    _Update_EducationDetails,
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
                                    _Insert_Education,
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
                    ////Extract instructor id from the registration number.
                    var instructorId = 
                        int.Parse(profile.RegistrationNumber.Substring(profile.RegistrationNumber.Length - 4));

                    var existingIds = 
                        (await db.QueryAsync<int>(_Select_ExistingInstructorExperienceIds, new { InstructorId = instructorId }, transaction )).ToList();

                    var incomingIds = profile.InstructorExperiences?
                        .Where(exp => exp.Id.HasValue)
                        .Select(exp => exp.Id.Value)
                        .ToList() ?? new List<int>();

                    var IdsToInactivate = existingIds.Except(incomingIds).ToList();

                    //Inactivate instructor's experience records
                    if (IdsToInactivate.Any())
                        await db.ExecuteAsync(_Update_InstructorExperience, new { Ids = IdsToInactivate }, transaction);

                    //Insert or Update instructor's experince records
                    if (profile.InstructorExperiences != null)
                    {
                        foreach (var exp in profile.InstructorExperiences)
                        {
                            if (exp.Id.HasValue)
                            {
                                await db.ExecuteAsync(
                                    _Update_InstructorExperienceDetails,
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
                                    _Insert_InstructorExperience,
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
