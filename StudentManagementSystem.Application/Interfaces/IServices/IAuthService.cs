using StudentManagementSystem.Application.DTOs.Auth;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterNewStudentAsync(StudentRegistrationDetailsDto dto);
        Task<string> RegisterNewInstructorAsync(InstructorRegistrationDetailsDto dto);
        Task<string> LoginAsync(string email, string password);
        Task<string> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<string> ResetPasswordAsync(ResetPasswordDto dto);
        Task<string> ChangePasswordAsync(ChangePasswordDto dto);
    }
}
