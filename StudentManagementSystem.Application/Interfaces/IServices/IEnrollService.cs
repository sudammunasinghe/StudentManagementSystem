using StudentManagementSystem.Application.DTOs.Enroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IEnrollService
    {
        Task EnrollStudentAsync(EnrollStudentDto dto);
    }
}
