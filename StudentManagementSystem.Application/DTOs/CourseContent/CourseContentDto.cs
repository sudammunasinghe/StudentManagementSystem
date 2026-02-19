using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.CourseContent
{
    public class CourseContentDto
    {
        public int? ContentId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
    }
}
