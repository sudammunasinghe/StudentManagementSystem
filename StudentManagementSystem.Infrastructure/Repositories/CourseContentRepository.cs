using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Infrastructure.Repositories
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public CourseContentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
