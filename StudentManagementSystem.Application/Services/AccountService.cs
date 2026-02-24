using StudentManagementSystem.Application.DTOs.Account;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        public AccountService(IAccountRepository accountRepository, ICurrentUserService currentUserService, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }

        public async Task<ProfileDetailsDto> GetProfileDetailsAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            var loggedUserRole = _currentUserService.Role;

            if (loggedUserId == null)
                throw new Exception("Unauthenticated user ...");

            var user =
                await _userRepository.GetUserByIdAsync(loggedUserId);

            var profileDetails = new ProfileDetailsDto
            {
                RegistrationNumber = null,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                ContactNumber = user.ContactNumber,
                Email = user.Email,
                NIC = user.NIC,
                EducationalDetails = null,
                InstructorExperienceDetails = null
            };

            if (loggedUserRole == "Student")
            {
                var educationalDetails =
                    await _accountRepository.GetEducationalDetailsByUserIdAsync(loggedUserId);

                profileDetails.EducationalDetails = educationalDetails
                    .Select(ed => new EducationalDetailsDto
                    {
                        Id = ed.Id,
                        StudentId = ed.StudentId,
                        Institute = ed.Institute,
                        Degree = ed.Degree,
                        Major = ed.Major,
                        StartingDate = ed.StartingDate,
                        EndingDate = ed.EndingDate,
                        IsStudying = ed.IsStudying,
                        Description = ed.Description
                    }).ToList();
            }
            else if (loggedUserRole == "Instructor")
            {
                var instructorExperienceDetails =
                    await _accountRepository.GetInstructorExperienceDetailsByUserIdAsync(loggedUserId);

                profileDetails.InstructorExperienceDetails = instructorExperienceDetails
                    .Select(ied => new InstructorExperienceDetailsDto
                    {
                        Id = ied.Id,
                        InstructorId = ied.InstructorId,
                        CompanyName = ied.CompanyName,
                        JobTitle = ied.JobTitle,
                        EmployementType = ied.EmployementType,
                        Location = ied.Location,
                        StartDate = ied.StartDate,
                        EndDate = ied.EndDate,
                        IsCurrentlyWorking = ied.IsCurrentlyWorking,
                        Description = ied.Description
                    }).ToList();
            }
            return profileDetails;
        }

        public async Task UpdateProfileDetailsAsync(UpdateProfileDetailsDto dto)
        {
            var loggedUserRole = _currentUserService.Role;
            var updatedProfile = new ProfileEntity
            {
                Id = dto.Id,
                RegistrationNumber = dto.RegistrationNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                NIC = dto.NIC,
                EducationalDetails = dto.EducationalDetails?
                    .Select(edu => new Education
                    {
                        Id = edu.Id,
                        StudentId = edu.StudentId,
                        Institute = edu.Institute,
                        Degree = edu.Degree,
                        Major = edu.Major,
                        StartingDate = edu.StartingDate,
                        EndingDate = edu.EndingDate,
                        IsStudying = edu.IsStudying,
                        Description = edu.Description
                    }).ToList() ?? new List<Education>(),
                InstructorExperiences = dto.InstructorExperienceDetails?
                    .Select(exp => new InstructorExperience
                    {
                        Id = exp.Id,
                        InstructorId = exp.InstructorId,
                        CompanyName = exp.CompanyName,
                        JobTitle = exp.JobTitle,
                        EmployementType = exp.EmployementType,
                        Location = exp.Location,
                        StartDate = exp.StartDate,
                        EndDate = exp.EndDate,
                        IsCurrentlyWorking = exp.IsCurrentlyWorking,
                        Description = exp.Description
                    }).ToList() ?? new List<InstructorExperience>()
            };
            await _accountRepository.UpdateProfileDetailsAsync(updatedProfile, loggedUserRole);
        }
    }
}
