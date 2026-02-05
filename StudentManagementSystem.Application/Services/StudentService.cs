using Microsoft.Identity.Client;
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

            var result = new StudentResponseDto
            {
                StudentId = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                Address = student.Address,
                Email = student.Email,
                NIC = student.NIC
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
            var existingStudent = await _studentRepository.GetStudentDetailsByStudentIdAsync(dto.Id);
            if (existingStudent == null)
                throw new Exception($"Student with Id {dto.Id} not foound ...");

            var updatedStudent = new Student
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                Email = dto.Email,
                NIC = dto.NIC
            };

            await _studentRepository.UpdateStudentDetailsAsync(updatedStudent);
            return true;
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
