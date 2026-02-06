using StudentManagementSystem.Application.DTOs.Enroll;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IEnrollService
    {
        Task EnrollStudentAsync(EnrollStudentDto dto);
    }
}
