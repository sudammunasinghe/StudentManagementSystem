using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Services
{
    public class CourseContentService : ICourseContentService
    {
        private readonly ICourseContentRepository _courseContentRepository;
        public CourseContentService(ICourseContentRepository courseContentRepository)
        {
            _courseContentRepository = courseContentRepository;
        }
    }
}
