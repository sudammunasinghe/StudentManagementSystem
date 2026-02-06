using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IEnrollRepository
    {
        Task<bool> IsEnrollmentExists(int studentId, int courseId);
        Task<int> EnrollStudentAsync(int studentId, int courseId);
    }
}
