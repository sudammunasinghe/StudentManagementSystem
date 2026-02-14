using StudentManagementSystem.Application.DTOs.CourseContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IServices
{
    public interface ICourseContentService
    {
        Task UploadCourseContentAsync(UploadCourseContentDto dto);
    }
}
