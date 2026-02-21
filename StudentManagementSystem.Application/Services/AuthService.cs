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
        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
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
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
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
            return "inserted";
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
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
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

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid credentials ...");
            return GenerateToken(user);
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
