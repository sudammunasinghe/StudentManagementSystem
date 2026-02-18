using SendGrid.Helpers.Errors.Model;
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

        public async Task<InstructorResponseDto> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(instructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

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
            var instructors =
                await _instructorRepository.GetAllInstructorsAsync();

            return instructors.Select(i => new InstructorResponseDto
            {
                InstructorId = i.Id,
                FullName = $"{i.FirstName} {i.LastName}",
                Email = i.Email,
                Address = i.Address,
                NIC = i.NIC
            }).ToList();
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

        public async Task<InstructorResponseDto> UpdateInstructorDetailsAsync(UpdateInstructorDto dto)
        {
            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(dto.Id);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            instructor.Update(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.NIC,
                dto.Address);

            var affectedRows =
                await _instructorRepository.UpdateInstructorDetailsAsync(instructor);

            if (affectedRows == 0)
                throw new Exception("Instructor update failed ...");

            return new InstructorResponseDto
            {
                InstructorId = instructor.Id,
                FullName = $"{instructor.FirstName} {instructor.LastName}",
                Email = instructor.Email,
                Address = instructor.Address,
                NIC = instructor.NIC
            };
        }

        public async Task InactivateInstructorByInstructorIdAsync(int instructorId)
        {
            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(instructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            var affectedRows =
                await _instructorRepository.InactivateInstructorByInstructorIdAsync(instructorId);

            if (affectedRows == 0)
                throw new Exception("Instructor inactivation failed...");
        }
    }
}
