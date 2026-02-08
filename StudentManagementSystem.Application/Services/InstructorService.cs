using StudentManagementSystem.Application.DTOs.Instructor;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<InstructorResponseDto?> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            var instructor = await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(instructorId);
            if (instructor == null)
                return null;

            return new InstructorResponseDto
            {
                InstructorId = instructor.Id,
                FullName = $"{instructor.FirstName} {instructor.LastName}",
                Email = instructor.Email,
                Address = instructor.Address,
                NIC = instructor.NIC
            };
        }

        public async Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync()
        {
            var instructors = await _instructorRepository.GetAllInstructorsAsync();
            return instructors?.Select(i => new InstructorResponseDto
            {
                InstructorId = i.Id,
                FullName = $"{i.FirstName} {i.LastName}",
                Email = i.Email,
                Address = i.Address,
                NIC = i.NIC
            }).ToList() ?? new List<InstructorResponseDto>();
        }

        public async Task<int> CreateInstructorAsync(CreateInstructorDto dto)
        {
            var newInstructor = Instructor.Create(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.NIC,
                dto.Address);

            return await _instructorRepository.CreateInstructorAsync(newInstructor);
        }

        public async Task<bool> UpdateInstructorDetailsAsync(UpdateInstructorDto dto)
        {
            var existingInstructor = await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(dto.Id);
            if (existingInstructor == null)
                return false;

            existingInstructor.Update(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.NIC,
                dto.Address);

            var affectedRows = await _instructorRepository.UpdateInstructorDetailsAsync(existingInstructor);
            return affectedRows > 0;
        }

        public async Task<bool> InactivateInstructorByInstructorIdAsync(int instructorId)
        {
            var instructor = await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(instructorId);
            if (instructor == null)
                return false;

            var affectedRows = await _instructorRepository.InactivateInstructorByInstructorIdAsync(instructorId);
            return affectedRows > 0;
        }
    }
}
