using Microsoft.Extensions.Configuration;
using StudentManagementSystem.Application.DTOs.Auth;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    true
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
                false,
                true
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
            return "inserted";
        }
    }
}
