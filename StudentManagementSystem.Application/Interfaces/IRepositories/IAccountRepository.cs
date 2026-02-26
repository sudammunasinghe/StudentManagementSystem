using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IAccountRepository
    {
        Task<List<Education>> GetEducationalDetailsByUserIdAsync(int userId);
        Task<List<InstructorExperience>> GetInstructorExperienceDetailsByUserIdAsync(int userId);
        Task UpdateProfileDetailsAsync(ProfileEntity profile, string role);
    }
}
