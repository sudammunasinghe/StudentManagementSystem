using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.CourseContent
{
    public class UploadCourseContentRequest
    {
        public int? CourseId { get; set; }
        public int? InstructorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? File { get; set; }
    }
}
