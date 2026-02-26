using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ITokenGeneratorService
    {
        string GenerateJwtToken(User user);
        string GeneratePasswordResetToken();
    }
}
