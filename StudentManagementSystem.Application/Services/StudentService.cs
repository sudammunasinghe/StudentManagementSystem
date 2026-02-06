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

        public async Task<StudentResponseDto> GetStudentDetailsByStudentIdAsync(int stdId)
        {
            var student = await _studentRepository.GetStudentDetailsByStudentIdAsync(stdId);
            if (student == null)
                throw new Exception($"Student with Id {stdId} not found ....");

            var enrolledCourses = await _studentRepository.GetEnrolledCoursesByStudentIdAsync(stdId);

            var result = new StudentResponseDto
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
                    Credits = ec.Credits
                }).ToList()
            };
            return result;
        }

        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            var result = students.Select(s => new StudentResponseDto
            {
                StudentId = s.Id,
                FullName = $"{s.FirstName} {s.LastName}",
                Address = s.Address,
                Email = s.Email,
                NIC = s.NIC
            }).ToList();
            return result;
        }

        public async Task<int> CreateStudentAsync(CreateStudentDto dto)
        {
            var newStudent = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Email = dto.Email,
                NIC = dto.NIC
            };
            return await _studentRepository.CreateStudentAsync(newStudent);
        }

        public async Task<bool> UpdateStudentDetailsAsync(UpdateStudentDto dto)
        {
            var student = await _studentRepository.GetStudentDetailsByStudentIdAsync(dto.Id);
            if (student == null)
                throw new Exception($"Student with Id {dto.Id} not foound ...");

            student.FirstName = dto.FirstName ?? student.FirstName;
            student.LastName = dto.LastName ?? student.LastName;
            student.Address = dto.Address ?? student.Address;
            student.Email = dto.Email ?? student.Email;
            student.NIC = dto.NIC ?? student.NIC;

            var affectedRows = await _studentRepository.UpdateStudentDetailsAsync(student);
            return affectedRows > 0;
        }

        public async Task<bool> IncativateStudentByStudentIdAsync(int stdId)
        {
            var affectedRows = await _studentRepository.IncativateStudentByStudentIdAsync(stdId);
            if (affectedRows == 0)
                throw new KeyNotFoundException("Student Id is not found ...");
            return true;
        }
    }
}
