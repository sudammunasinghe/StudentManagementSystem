using StudentManagementSystem.Domain.Entities;
using System.Collections;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentDetailsByStudentIdAsync(int stdId);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<int> CreateStudentAsync(Student newStudent);
        Task<int> UpdateStudentDetailsAsync(Student updatedStudent);
        Task<int> IncativateStudentByStudentIdAsync(int stdId);
        Task<IEnumerable<Course>> GetEnrolledCoursesByStudentIdAsync(int studentId);
    }
}
