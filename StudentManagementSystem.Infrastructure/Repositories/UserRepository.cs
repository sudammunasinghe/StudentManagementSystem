using Dapper;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Entities;
using StudentManagementSystem.Domain.Persistence;
using StudentManagementSystem.Infrastructure.Persistence.Sql.Helpers;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISqlQueryLoader _sqlQueryLoader;

        private readonly string _Select_UserDetailsByEmail;
        private readonly string _Select_UserDetailsByUserId;
        private readonly string _Insert_NewUser;
        private readonly string _Insert_NewStudent;
        private readonly string _Update_UserForRegistrationNumber;
        private readonly string _Insert_NewEducation;
        private readonly string _Insert_NewInstructor;
        private readonly string _Insert_NewInstructorExperience;
        private readonly string _Update_UserForPasswordResetToken;
        private readonly string _Select_UserDetailsByToken;
        private readonly string _Update_UserForPassword;
        public UserRepository(IDbConnectionFactory connectionFactory, ISqlQueryLoader sqlQueryLoader)
        {
            _connectionFactory = connectionFactory;
            _sqlQueryLoader = sqlQueryLoader;
            _Select_UserDetailsByEmail = _sqlQueryLoader.Load("User", "Select_UserDetailsByEmail.sql");
            _Select_UserDetailsByUserId = _sqlQueryLoader.Load("User", "Select_UserDetailsByUserId.sql");
            _Insert_NewUser = _sqlQueryLoader.Load("User", "Insert_NewUser.sql");
            _Insert_NewStudent = _sqlQueryLoader.Load("User", "Insert_NewStudent.sql");
            _Update_UserForRegistrationNumber = _sqlQueryLoader.Load("User", "Update_UserForRegistrationNumber.sql");
            _Insert_NewEducation = _sqlQueryLoader.Load("User", "Insert_NewEducation.sql");
            _Insert_NewInstructor = _sqlQueryLoader.Load("User", "Insert_NewInstructor.sql");
            _Insert_NewInstructorExperience = _sqlQueryLoader.Load("User", "Insert_NewInstructorExperience.sql");
            _Update_UserForPasswordResetToken = _sqlQueryLoader.Load("User", "Update_UserForPasswordResetToken.sql");
            _Select_UserDetailsByToken = _sqlQueryLoader.Load("User", "Select_UserDetailsByToken.sql");
            _Update_UserForPassword = _sqlQueryLoader.Load("User", "Update_UserForPassword.sql");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(_Select_UserDetailsByEmail, new { Email = email });
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User?>(_Select_UserDetailsByUserId, new { UserId = userId });
        }

        public async Task<int> CreateNewStudentUserAsync(User newUser, Student studentDetails)
        {
            using var db = _connectionFactory.CreateConnection();
            db.Open();

            using var transaction = db.BeginTransaction();
            try
            {
                var userId = await db.ExecuteScalarAsync<int>(
                    _Insert_NewUser,
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

                var studentId = await db.ExecuteScalarAsync<int>(
                    _Insert_NewStudent,
                    new { UserId = userId, GPA = studentDetails.GPA },
                    transaction
                );

                //Generate registration number and update the user table
                var registrationNumber = $"STD/{DateTime.UtcNow.Year}/{studentId:D4}";
                await db.ExecuteAsync(_Update_UserForRegistrationNumber, new { UserId = userId, RegistrationNumber = registrationNumber }, transaction);

                if (studentDetails.EducationDetails != null && studentDetails.EducationDetails.Any())
                {
                    foreach (var edu in studentDetails.EducationDetails)
                    {
                        await db.ExecuteAsync(
                            _Insert_NewEducation,
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
                var userId = await db.ExecuteScalarAsync<int>(
                    _Insert_NewUser,
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

                var instructorId = await db.ExecuteScalarAsync<int>(
                    _Insert_NewInstructor,
                    new { UserId = userId, ExperienceYears = instructorDetails.ExperienceYears, PreferredSalary = instructorDetails.PreferredSalary },
                    transaction
                );

                //Generate registration number and update the user table
                var registrationNumber = $"INS/{DateTime.UtcNow.Year}/{instructorId:D4}";
                await db.ExecuteAsync(_Update_UserForRegistrationNumber, new { UserId  = userId, RegistrationNumber = registrationNumber }, transaction );

                if (instructorDetails.InstructorExperiences != null && instructorDetails.InstructorExperiences.Any())
                {
                    foreach (var exp in instructorDetails.InstructorExperiences)
                    {
                        await db.ExecuteAsync(
                            _Insert_NewInstructorExperience,
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
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(_Update_UserForPasswordResetToken, new { UserId = userId, Token = token, Expiry = expiry });
        }

        public async Task<User?> GetUserByResetTokenAsync(string token)
        {
            using var db = _connectionFactory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<User>(_Select_UserDetailsByToken, new { Token = token });
        }

        public async Task UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            using var db = _connectionFactory.CreateConnection();
            await db.ExecuteAsync(_Update_UserForPassword, new { UserId = userId, NewPasswordHash = newPasswordHash });
        }
    }
}
