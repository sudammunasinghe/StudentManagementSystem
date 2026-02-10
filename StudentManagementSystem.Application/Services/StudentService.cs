using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.Student;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;

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
            var student = await _studentRepository.GetStudentDetailsByStudentIdAsync(stdId);
            if (student == null)
                return null;

            var enrolledCourses = await _studentRepository.GetEnrolledCoursesByStudentIdAsync(stdId);

            return new StudentResponseDto
            {
                StudentId = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Address = student.Address,
                Email = student.Email,
                NIC = student.NIC,
                EnrolledCourses = enrolledCourses.Select(ec => new CourseDto
                {
                    CourseId = ec.Id,
                    Title = ec.Title,
                    Credits = ec.Credits,
                    EnrollmentStatus = ec.Status
                }).ToList() ?? new List<CourseDto>()
            } ?? new StudentResponseDto();
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            return students.Select(s => new StudentResponseDto
            {
                StudentId = s.Id,
                FullName = $"{s.FirstName} {s.LastName}",
                Address = s.Address,
                Email = s.Email,
                NIC = s.NIC
            }).ToList() ?? new List<StudentResponseDto>();
        }

        public async Task<int> CreateStudentAsync(CreateStudentDto dto)
        {
            var newStudent = Student.Create(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.Email,
                dto.NIC);
            return await _studentRepository.CreateStudentAsync(newStudent);
        }

        public async Task<bool> UpdateStudentDetailsAsync(UpdateStudentDto dto)
        {
            var existingStudent = await _studentRepository.GetStudentDetailsByStudentIdAsync(dto.Id);
            if (existingStudent == null)
                return false;

            existingStudent.Update(
                dto.FirstName,
                dto.LastName,
                dto.Address,
                dto.Email,
                dto.NIC);

            var affectedRows = await _studentRepository.UpdateStudentDetailsAsync(existingStudent);
            return affectedRows > 0;
        }

        public async Task<bool> InactivateStudentByStudentIdAsync(int stdId)
        {
            var student = await _studentRepository.GetStudentDetailsByStudentIdAsync(stdId);
            if (student == null)
                return false;

            var affectedRows = await _studentRepository.InactivateStudentByStudentIdAsync(stdId);
            return affectedRows > 0;
        }
    }
}
