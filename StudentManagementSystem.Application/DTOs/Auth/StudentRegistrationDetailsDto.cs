using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.Auth
{
    public class StudentRegistrationDetailsDto : RegistrationDetailsDto
    {
        public double? GPA { get; set; }
    }
}
