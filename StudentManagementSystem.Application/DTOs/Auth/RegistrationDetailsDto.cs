using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.Auth
{
    public class RegistrationDetailsDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public string? ConatctNumber { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
    }
}
