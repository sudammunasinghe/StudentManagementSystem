using StudentManagementSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.DTOs.Approval
{
    public class ApprovalCompletionDto
    {
        public EnrollmentStatus Status { get; set; }
        public string? RejectedReason { get; set; }
    }
}
