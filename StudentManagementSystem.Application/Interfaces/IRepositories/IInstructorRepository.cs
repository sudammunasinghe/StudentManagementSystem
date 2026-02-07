using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IInstructorRepository
    {
        Task<Instructor> GetInstructorDetailsByInstructorIdAsync(int instructorId);
        Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
        Task<int> CreateInstructorAsync(Instructor newInstructor);
    }
}
