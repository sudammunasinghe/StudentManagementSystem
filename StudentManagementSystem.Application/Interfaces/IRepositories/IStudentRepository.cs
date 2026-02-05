using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentDetailsByStudentIdAsync(int stdId);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<int> CreateStudentAsync(Student newStudent);
        Task UpdateStudentDetailsAsync(Student updatedStudent);
        Task<int> IncativateStudentByStudentIdAsync(int stdId);
    }
}
