using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Interfaces.IRepositories
{
    public interface IInstructorRepository
    {
        Task<(List<Course>? courses, List<CourseContent>? contents)> GetCoursesByInstructorAsync(int instructorId);
        Task<Instructor?> GetInstructorDetailsByUserIdAsync(int userId);
        Task CreateNewCourseAsync(Course newCourse);
        Task<Course?> GetCourseDetailsByCourseIdAsync(int courseId);
        Task UploadCourseContentAsync(CourseContent newContent);
    }
}
