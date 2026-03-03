using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Get user details by User's Email.
        /// </summary>
        /// <param name="email">Logged user's email address</param>
        /// <returns>A <see cref="User"/> object if a matching user is found. otherwise null</returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Get user details by User's Id.
        /// </summary>
        /// <param name="userId">Logged User's Id</param>
        /// <returns>A <see cref="User"/> object if a matching user is found. otherwise null</returns>
        Task<User?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Create new user with the student role.
        /// </summary>
        /// <param name="newUser">The user entity containing basic user details.</param>
        /// <param name="studentDetails"></param>
        /// <returns></returns>
        Task<int> CreateNewStudentUserAsync(User newUser, Student studentDetails);

        /// <summary>
        /// Create new user as a instructor.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="instructorDetails"></param>
        /// <returns></returns>
        Task<int> CreateNewInstructorUserAsync(User newUser, Instructor instructorDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task SavePasswordResetTokenAsync(int userId, string token, DateTime expiry);

        /// <summary>
        /// Get user details by user's token
        /// </summary>
        /// <param name="token">Token for authenticated user.</param>
        /// <returns>A <see cref="User"/> object if a matching user is found. otherwise null</returns>
        Task<User?> GetUserByResetTokenAsync(string token);

        /// <summary>
        /// Update user's password.
        /// </summary>
        /// <param name="userId">Logged user's Id</param>
        /// <param name="newPasswordHash"></param>
        /// <returns></returns>
        Task UpdatePasswordAsync(int userId, string newPasswordHash);
    }
}
