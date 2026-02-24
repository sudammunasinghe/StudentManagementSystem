using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.Account
{
    public class UpdateProfileDetailsDto : ProfileDetailsDto
    {
        public int? Id { get; set; }
    }
}
