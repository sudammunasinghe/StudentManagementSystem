using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.Instructor
{
    public class UpdateInstructorDto : CreateInstructorDto
    {
        public int Id { get; set; }
    }
}
