using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface IInstructorService
    {
        Task<IEnumerable<CourseDto>> GetCoursesByInstructorAsync();
        Task CreateNewCourseAsync(CreateCourseDto dto);
        Task UploadCourseContentAsync(UploadCourseContentDto dto);
    }
}
