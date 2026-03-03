using StudentManagementSystem.Application.DTOs.Account;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IAccountService
    {
        /// <summary>
        /// Retrieves the profile details of a specific user.
        /// </summary>
        /// <returns><see cref="ProfileDetailsDto"/>Containing the user's profile details</returns>
        Task<ProfileDetailsDto> GetProfileDetailsAsync();

        /// <summary>
        /// Updates the profile details of a user.
        /// </summary>
        /// <param name="dto">An object containing updated profile details.</param>
        /// <returns></returns>
        Task UpdateProfileDetailsAsync(UpdateProfileDetailsDto dto);
    }
}
