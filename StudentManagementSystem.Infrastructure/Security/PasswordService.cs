using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string? password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string? password, string? hashValue)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashValue);
        }
    }
}
