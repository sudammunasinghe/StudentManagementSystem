using StudentManagementSystem.Application.DTOs.Student;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IStudentService
    {
        Task<StudentResponseDto> GetStudentDetailsByStudentIdAsync(int stdId);
        Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
        Task<int> CreateStudentAsync(CreateStudentDto dto);
        Task<StudentResponseDto> UpdateStudentDetailsAsync(UpdateStudentDto dto);
        Task InactivateStudentByStudentIdAsync(int stdId);
    }
}
