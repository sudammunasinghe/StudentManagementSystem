using StudentManagementSystem.Application.DTOs.Instructor;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IInstructorServiceTemp
    {
        Task<InstructorResponseDto> GetInstructorDetailsByInstructorIdAsync(int instructorId);
        Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync();
        Task<int> CreateInstructorAsync(CreateInstructorDto dto);
        Task<InstructorResponseDto> UpdateInstructorDetailsAsync(UpdateInstructorDto dto);
        Task InactivateInstructorByInstructorIdAsync(int instructorId);
    }
}
