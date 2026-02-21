using StudentManagementSystem.Application.DTOs.Auth;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterNewStudentAsync(StudentRegistrationDetailsDto dto);
        Task<string> RegisterNewInstructorAsync(InstructorRegistrationDetailsDto dto);
        Task<string> LoginAsync(string email, string password);
    }
}
