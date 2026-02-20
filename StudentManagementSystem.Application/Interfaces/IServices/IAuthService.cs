using StudentManagementSystem.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterNewStudentAsync(StudentRegistrationDetailsDto dto);
        Task<string> RegisterNewInstructorAsync(InstructorRegistrationDetailsDto dto);
        //Task<string> LoginAsync(string email, string password);
    }
}
