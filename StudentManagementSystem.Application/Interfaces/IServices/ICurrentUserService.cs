namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? Email { get; }
        string? Role { get; }
    }
}
