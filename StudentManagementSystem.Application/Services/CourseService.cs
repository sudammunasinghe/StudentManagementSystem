using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto> GetCourseDetailsByCourseIdAsync(int courseId)
        {
            var course = await _courseRepository.GetCourseDetailsByCourseIdAsync(courseId);
            if (course == null)
                throw new Exception($"Course with Id {courseId} not found ...");

            var result = new CourseDto
            {
                CourseId = course.Id,
                Title = course.Title,
                Credits = course.Credits
            };
            return result;

        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCoursesAsync();
            var result = courses.Select(c => new CourseDto
            {
                CourseId = c.Id,
                Title = c.Title,
                Credits = c.Credits
            }).ToList();
            return result;
        }

        public async Task<int> CreateNewCourseAsync(CreateCourseDto dto)
        {
            var newCourse = new Course
            {
                Title = dto.Title,
                Credits = dto.Credits
            };
            return await _courseRepository.CreateNewCourseAsync(newCourse);
        }

        public async Task<bool> UpdateCourseDetailsAsync(UpdateCourseDto dto)
        {
            var course = await _courseRepository.GetCourseDetailsByCourseIdAsync(dto.Id);
            if (course == null)
                throw new Exception("Invalid course Id ...");

            course.Title = dto.Title ?? course.Title;
            course.Credits = dto.Credits ?? course.Credits;
            var affectedRows = await _courseRepository.UpdateCourseDetailsAsync(course);
            return affectedRows > 0;
        }

        public async Task<bool> InactivateCourseByCourseIdAsync(int courseId)
        {
            var affectedRows = await _courseRepository.InactivateCourseByCourseIdAsync(courseId);
            if (affectedRows == 0)
                throw new KeyNotFoundException("Course Id not found ...");
            return true;
        }
    }
}
