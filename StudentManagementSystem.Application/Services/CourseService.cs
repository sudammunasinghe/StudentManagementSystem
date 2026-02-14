using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        public CourseService(ICourseRepository courseRepository, IInstructorRepository instructorRepository)
        {
            _courseRepository = courseRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<CourseDto> GetCourseDetailsByCourseIdAsync(int courseId)
        {
            var course = 
                await _courseRepository.GetCourseDetailsByCourseIdAsync(courseId);

            var studentCount = 
                await _courseRepository.GetEnrolledStudentCountForCourseByCourseIdAsync(courseId);

            if (course == null)
                throw new NotFoundException("Course not found ...");

            return new CourseDto
            {
                CourseId = course.Id,
                Title = course.Title,
                Credits = course.Credits,
                NoOfEnrolledStudents = studentCount
            };
        }

        public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<int> CreateNewCourseAsync(CreateCourseDto dto)
        {
            var instructor = 
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(dto.InstructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            var newCourse = Course.Create(
                dto.Title, 
                dto.Credits,
                dto.InstructorId
             );
            return await _courseRepository.CreateNewCourseAsync(newCourse);
        }

        public async Task<CourseDto> UpdateCourseDetailsAsync(UpdateCourseDto dto)
        {
            var course = 
                await _courseRepository.GetCourseDetailsByCourseIdAsync(dto.Id);

            if (course == null)
                throw new NotFoundException("Course not found ...");

            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(dto.InstructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            course.Update(
                dto.Title,
                dto.Credits,
                dto.InstructorId
            );

            var affectedRows = 
                await _courseRepository.UpdateCourseDetailsAsync(course);

            if (affectedRows == 0)
                throw new Exception("Course update failed ...");

            return new CourseDto
            {
                CourseId = course.Id,
                Title = course.Title,
                Credits = course.Credits
            };
            
        }

        public async Task InactivateCourseByCourseIdAsync(int courseId)
        {
            var course = 
                await _courseRepository.GetCourseDetailsByCourseIdAsync(courseId);

            if (course == null)
                throw new NotFoundException("Course not found ...");

            var affectedRows = 
                await _courseRepository.InactivateCourseByCourseIdAsync(courseId);

            if (affectedRows == 0)
                throw new Exception("Course incativation failed ...");
        }
    }
}
