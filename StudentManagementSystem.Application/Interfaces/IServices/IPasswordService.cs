namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IPasswordService
    {
        string HashPassword(string? password);
        bool VerifyPassword(string? password, string? hashValue);
    }
}
