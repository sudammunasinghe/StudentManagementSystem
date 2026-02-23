using StudentManagementSystem.Application.DTOs.Auth;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public AuthService(IUserRepository userRepository, IPasswordService passwordService, ICurrentUserService currentUserService, ITokenGeneratorService tokenGeneratorService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _currentUserService = currentUserService;
            _tokenGeneratorService = tokenGeneratorService;
        }

        public async Task<string> RegisterNewStudentAsync(StudentRegistrationDetailsDto dto)
        {
            var existingUser =
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (existingUser != null)
                throw new Exception("User already exists ...");

            var newUser = User.Create(
                    dto.FirstName,
                    dto.LastName,
                    dto.Address,
                    dto.NIC,
                    dto.ConatctNumber,
                    dto.Email,
                    dto.Password,
                    (int)Roles.Student
            );
            var passwordHash = _passwordService.HashPassword(dto.Password);
            newUser.PasswordHash = passwordHash;

            var studentDetails = Student.Create(dto.GPA);
            if (dto.EducationalDetails != null && dto.EducationalDetails.Any())
            {
                foreach (var edu in dto.EducationalDetails)
                {
                    studentDetails.EducationDetails.Add(new Education
                    {
                        Institute = edu.Institute,
                        Degree = edu.Degree,
                        Major = edu.Major,
                        StartingDate = edu.StartingDate,
                        EndingDate = edu.EndingDate,
                        IsStudying = edu.IsStudying,
                        Description = edu.Description
                    });
                }
            }
            await _userRepository.CreateNewStudentUserAsync(newUser, studentDetails);
            return _tokenGeneratorService.GenerateJwtToken(newUser);
        }

        public async Task<string> RegisterNewInstructorAsync(InstructorRegistrationDetailsDto dto)
        {
            var existingUser =
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (existingUser != null)
                throw new Exception("User already exists ...");

            var newUser = User.Create(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.NIC,
                dto.ConatctNumber,
                dto.Email,
                dto.Password,
                (int)Roles.Instructor
            );
            var passwordHash = _passwordService.HashPassword(dto.Password);
            newUser.PasswordHash = passwordHash;

            var instructorDetails = Instructor.Create(
                dto.ExperienceYears,
                dto.PreferredSalary
            );

            if (dto.InstructorExperienceDetails != null && dto.InstructorExperienceDetails.Any())
            {
                foreach (var exp in dto.InstructorExperienceDetails)
                {
                    instructorDetails.InstructorExperiences.Add(new InstructorExperience
                    {
                        CompanyName = exp.CompanyName,
                        JobTitle = exp.JobTitle,
                        EmployementType = exp.EmployementType,
                        Location = exp.Location,
                        StartDate = exp.StartDate,
                        EndDate = exp.EndDate,
                        IsCurrentlyWorking = exp.IsCurrentlyWorking,
                        Description = exp.Description

                    });
                }
            }

            await _userRepository.CreateNewInstructorUserAsync(newUser, instructorDetails);
            return _tokenGeneratorService.GenerateJwtToken(newUser);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user =
                await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid credentials ...");
            return _tokenGeneratorService.GenerateJwtToken(user);
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user =
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return "If the email exists, a reset link has been sent...";

            var token = _tokenGeneratorService.GeneratePasswordResetToken();
            var expiry = DateTime.Now.AddMinutes(5);

            await _userRepository.SavePasswordResetTokenAsync(user.Id, token, expiry);
            return "If the email exists, a reset link has been sent...";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user =
                await _userRepository.GetUserByResetTokenAsync(dto.Token);

            if (user == null ||
                user.PasswrodResetToken == null ||
                user.PasswrodResetTokenExpiry < DateTime.Now
             )
                throw new Exception("Invalid or expired reset token ...");

            var newPasswordHash = _passwordService.HashPassword(dto.NewPassword);
            await _userRepository.UpdatePasswordAsync(user.Id, newPasswordHash);
            return "Password has been reset successfully ...";
        }

        public async Task<string> ChangePasswordAsync(ChangePasswordDto dto)
        {
            int loggedUserId = _currentUserService.UserId;
            var loggedUser = await _userRepository.GetUserByIdAsync(loggedUserId);

            if (loggedUser == null)
                throw new Exception("Unauthenticated User ...");

            if (!_passwordService.VerifyPassword(dto.CurrentPassword, loggedUser.PasswordHash))
                throw new Exception("Current password is incorrect ...");

            if (_passwordService.VerifyPassword(dto.NewPassword, loggedUser.PasswordHash))
                throw new Exception("New password should be differ from the current password ...");

            var newPasswordHash = _passwordService.HashPassword(dto.NewPassword);

            await _userRepository.UpdatePasswordAsync(loggedUser.Id, newPasswordHash);
            return "Your password changed successfully ...";
        }
    }
}
