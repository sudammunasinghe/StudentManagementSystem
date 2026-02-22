using Microsoft.AspNetCore.Http;
using StudentManagementSystem.Application.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StudentManagementSystem.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    throw new Exception("Unauthorized ...");

                return int.Parse(userIdClaim.Value);
            }
        }

        public string? Email =>
            _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.Email)?.Value;

        public string? Role =>
            _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.Role)?.Value;
    }
}
