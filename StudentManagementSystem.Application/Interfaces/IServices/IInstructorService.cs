using StudentManagementSystem.Application.DTOs.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IInstructorService
    {
        Task<InstructorResponseDto> GetInstructorDetailsByInstructorIdAsync(int instructorId);
        Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync();
        Task<int> CreateInstructorAsync(CreateInstructorDto dto);
    }
}
