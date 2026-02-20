using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateNewStudentUserAsync(User newUser, Student studentDetails);
        Task<int> CreateNewInstructorUserAsync(User newUser, Instructor instructorDetails);
    }
}
