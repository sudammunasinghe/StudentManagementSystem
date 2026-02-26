using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
        Task<int> CreateNewStudentUserAsync(User newUser, Student studentDetails);
        Task<int> CreateNewInstructorUserAsync(User newUser, Instructor instructorDetails);
        Task SavePasswordResetTokenAsync(int userId, string token, DateTime expiry);
        Task<User?> GetUserByResetTokenAsync(string token);
        Task UpdatePasswordAsync(int userId, string newPasswordHash);
    }
}
