using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IInstructorRepository _instructorRepository;
        public StudentService(IStudentRepository studentRepository, ICurrentUserService currentUserService, IInstructorRepository instructorRepository)
        {
            _studentRepository = studentRepository;
            _currentUserService = currentUserService;
            _instructorRepository = instructorRepository;
        }

        public async Task EnrollToCourseAsync(int courseId)
        {

            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("UnAuthorized User ...");

            var course =
                await _instructorRepository.GetCourseDetailsByCourseIdAsync(courseId);
            if (course == null)
                throw new NotFoundException("Course not found ...");

            var student =
                await _studentRepository.GetStudentByUserIdAsync(loggedUserId);
            if (student == null)
                throw new UnauthorizedAccessException("User is not a valid student ...");

            var enrollment =
                await _studentRepository.GetEnrollmentDetailsAsync(student.Id, courseId);

            if (enrollment != null && enrollment.Status == EnrollmentStatus.Pending)
                throw new Exception($"Pending approval already exits for course : {course.Title}");

            if (enrollment != null && (enrollment.Status == EnrollmentStatus.Approved || enrollment.Status == EnrollmentStatus.Active))
                throw new Exception($"You already enrolled the course : {course.Title}");

            await _studentRepository.EnrollToCourseAsync(student.Id, courseId);
        }
    }
}
