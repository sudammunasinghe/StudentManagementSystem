using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;
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

        public async Task<IEnumerable<CourseDto>> GetAllEnrolledCoursesAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("UnAuthorized User ...");

            var enrolledCourseDetails =
                await _studentRepository.GetAllEnrolledCoursesByUserIdAsync(loggedUserId);

            return enrolledCourseDetails.courses
                .Select(c => new CourseDto
                {
                    CourseId = c.Id,
                    Credits = c.Credits,
                    Title = c.Title,
                    Description = c.Description,
                    CategoryEnum = c.CategoryEnum,
                    DurationHours = c.DurationHours,
                    EntrollmentLimit = c.EntrollmentLimit,
                    CourseContents = enrolledCourseDetails.courseContents
                        .Where(cc => cc.CourseId == c.Id)
                        .Select(cc => new CourseContentDto
                        {
                            ContentId = cc.Id,
                            Title = cc.Title,
                            Description = cc.Description,
                            ContentType = cc.ContentType,
                            FileSize = cc.FileSize,

                        }).ToList()

                }).ToList();
        }
    }
}
