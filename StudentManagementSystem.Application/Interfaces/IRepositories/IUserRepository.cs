using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateNewStudentUserAsync(User newUser, Student studentDetails);
        Task<int> CreateNewInstructorUserAsync(User newUser, Instructor instructorDetails);
    }
}
