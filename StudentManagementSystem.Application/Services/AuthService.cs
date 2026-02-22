using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagementSystem.Application.DTOs.Auth;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly ICurrentUserService _currentUserService;
        public AuthService(IUserRepository userRepository, IConfiguration configuration, IPasswordService passwordService, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordService = passwordService;
            _currentUserService = currentUserService;
        }

        public async Task<string> RegisterNewStudentAsync(StudentRegistrationDetailsDto dto)
        {
            var existingUser =
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (existingUser != null)
                throw new Exception("User already exists ...");

            var newUser = User.Create(
                    dto.Email,
                    dto.Password,
                    (int)Roles.Student
            );
            var passwordHash = _passwordService.HashPassword(dto.Password);
            newUser.PasswordHash = passwordHash;

            var studentDetails = Student.Create(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.ConatctNumber,
                dto.NIC,
                dto.GPA
            );
            await _userRepository.CreateNewStudentUserAsync(newUser, studentDetails);
            return GenerateToken(newUser);
        }

        public async Task<string> RegisterNewInstructorAsync(InstructorRegistrationDetailsDto dto)
        {
            var existingUser =
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (existingUser != null)
                throw new Exception("User already exists ...");

            var newUser = User.Create(
                dto.Email,
                dto.Password,
                (int)Roles.Instructor
            );
            var passwordHash = _passwordService.HashPassword(dto.Password);
            newUser.PasswordHash = passwordHash;

            var instructorDetails = Instructor.Create(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.ConatctNumber,
                dto.NIC,
                dto.ExperienceYears,
                dto.PreferredSalary
            );

            await _userRepository.CreateNewInstructorUserAsync(newUser, instructorDetails);
            return GenerateToken(newUser);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user =
                await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !_passwordService.VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid credentials ...");
            return GenerateToken(user);
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = 
                await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return "If the email exists, a reset link has been sent...";

            var token = TokenGenerator.GenerateToken();
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

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, ((Roles)user.RoleId).ToString())
            };

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
