using StudentManagementSystem.Application.DTOs.Enroll;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Application.Services
{
    public class EnrollService : IEnrollService
    {
        private readonly IEnrollRepository _enrollRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        public EnrollService(IEnrollRepository enrollRepository, IStudentRepository studentRepository, ICourseRepository courseRepository)
        {
            _enrollRepository = enrollRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
        }

        public async Task EnrollStudentAsync(EnrollStudentDto dto)
        {
            var student = await _studentRepository.GetStudentDetailsByStudentIdAsync(dto.StudentId);
            if (student == null)
                throw new Exception($"Student With Id {dto.StudentId} not exists ...");

            var course = await _courseRepository.GetCourseDetailsByCourseIdAsync(dto.CourseId);
            if (course == null)
                throw new Exception($"Course with Id {dto.CourseId} not exists ...");

            if (await _enrollRepository.IsEnrollmentExists(dto.StudentId, dto.CourseId))
                throw new Exception("Already Exists ...");

            await _enrollRepository.EnrollStudentAsync(dto.StudentId, dto.CourseId);
        }
    }
}
