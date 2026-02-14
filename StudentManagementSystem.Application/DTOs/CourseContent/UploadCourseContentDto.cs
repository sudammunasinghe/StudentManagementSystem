using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.CourseContent
{
    public class UploadCourseContentDto
    {
        public int? CourseId { get; set; }
        public int? InstructorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public byte[]? FileBytes { get; set; }
        public string? FileName { get; set; }
    }
}
