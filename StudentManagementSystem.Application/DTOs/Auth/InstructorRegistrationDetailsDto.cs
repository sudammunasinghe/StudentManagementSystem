using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.Auth
{
    public class InstructorRegistrationDetailsDto : RegistrationDetailsDto
    {
        public int ExperienceYears { get; set; }
        public decimal? PreferredSalary { get; set; }

    }
}
