using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Retrieves all educational details associated with the specific user
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of <see cref="Education"/>records linked to the specific user.
        /// Returns empty list, if no records are found.
        /// </returns>
        Task<List<Education>> GetEducationalDetailsByUserIdAsync(int userId);

        /// <summary>
        /// Get instructor's experience details by user id.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of <see cref="InstructorExperience"/>records linked to the specific user.
        /// Returns empty list, if no records are found.
        /// </returns>
        Task<List<InstructorExperience>> GetInstructorExperienceDetailsByUserIdAsync(int userId);

        /// <summary>
        /// Updates the profile details of a user based on their role.
        /// </summary>
        /// <param name="profile">The profile entity containing updated profile details</param>
        /// <param name="role">The role of the user</param>
        /// <returns></returns>
        Task UpdateProfileDetailsAsync(ProfileEntity profile, string role);
    }
}
