using StudentManagementSystem.Application.DTOs.Account;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IAccountService
    {
        Task<ProfileDetailsDto> GetProfileDetailsAsync();
    }
}
