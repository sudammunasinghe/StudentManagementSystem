using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<StudentResponseDto?> GetStudentDetailsByStudentIdAsync(int stdId)
        {
            var student =
                await _studentRepository.GetStudentDetailsByStudentIdAsync(stdId);

            if (student == null)
                throw new NotFoundException("Student not found ...");

            var enrolledCourses =
                await _studentRepository.GetEnrolledCoursesByStudentIdAsync(stdId);

            var enrolledCourseContents =
                await _studentRepository.GetCourseContentsByStudentIdAsync(stdId);

            return new StudentResponseDto
            {
                StudentId = student.Id,
                EnrolledCourses = enrolledCourses.Select(ec => new CourseDto
                {
                    CourseId = ec.Id,
                    Title = ec.Title,
                    Credits = ec.Credits,
                    CourseContents = enrolledCourseContents
                        .Where(cc => cc.CourseId == ec.Id)
                        .Select(cc => new CourseContentDto
                        {
                            ContentId = cc.Id,
                            Title = cc.Title,
                            Description = cc.Description,
                            ContentType = cc.ContentType,
                            FileSize = cc.FileSize
                        }).ToList() ?? new List<CourseContentDto>()
                }).ToList()
            };
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students =
                await _studentRepository.GetAllStudentsAsync();

            return students.Select(s => new StudentResponseDto
            {
                StudentId = s.Id
            }).ToList();
        }

        public async Task<int> CreateStudentAsync(CreateStudentDto dto)
        {
            //var newStudent = Student.Create(
            //    dto.FirstName,
            //    dto.LastName,
            //    dto.Address,
            //    dto.NIC);

            //return await _studentRepository.CreateStudentAsync(newStudent);
            return 1;
        }

        public async Task<StudentResponseDto> UpdateStudentDetailsAsync(UpdateStudentDto dto)
        {
            var student =
                await _studentRepository.GetStudentDetailsByStudentIdAsync(dto.Id);

            if (student == null)
                throw new NotFoundException("Student not found ...");

            student.Update(
                3.00
                );

            var affectedRows =
                await _studentRepository.UpdateStudentDetailsAsync(student);

            if (affectedRows == 0)
                throw new Exception("Student update failed ...");

            return new StudentResponseDto
            {
                StudentId = dto.Id,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Address = dto.Address,
                Email = dto.Email,
                NIC = dto.NIC
            };
        }

        public async Task InactivateStudentByStudentIdAsync(int stdId)
        {
            var student =
                await _studentRepository.GetStudentDetailsByStudentIdAsync(stdId);

            if (student == null)
                throw new NotFoundException("Student not found ...");

            var affectedRows =
                await _studentRepository.InactivateStudentByStudentIdAsync(stdId);

            if (affectedRows == 0)
                throw new Exception("Student inactivation failed ...");
        }
    }
}
