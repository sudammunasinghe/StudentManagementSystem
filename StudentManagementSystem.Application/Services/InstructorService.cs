using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly ICurrentUserService _currentUserService;
        public InstructorService(IInstructorRepository instructorRepository, ICurrentUserService currentUserService)
        {
            _instructorRepository = instructorRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<CourseDto>> GetCoursesByInstructorAsync()
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized user ...");

            var instructor = 
                await _instructorRepository.GetInstructorDetailsByUserIdAsync(loggedUserId);

            if (instructor == null)
                throw new UnauthorizedAccessException("User is not a valid instructor ...");

            var courseDetails = 
                await _instructorRepository.GetCoursesByInstructorAsync(instructor.Id);

            return courseDetails.courses.
                Select(c => new CourseDto
                {
                    CourseId = c.Id,
                    Credits = c.Credits,
                    Title = c.Title,
                    Description = c.Description,
                    CategoryEnum = c.CategoryEnum,
                    DurationHours = c.DurationHours,
                    EntrollmentLimit = c.EntrollmentLimit,
                    CourseContents = courseDetails.contents
                        .Where(cn => cn.CourseId == c.Id)
                        .Select(cn => new CourseContentDto
                        {
                            ContentId = cn.Id,
                            Title = cn.Title,
                            Description = cn.Description,
                            ContentType = cn.ContentType,
                            FileSize = cn.FileSize
                        }).ToList() ?? new List<CourseContentDto>()
                }).ToList();
        }

        public async Task CreateNewCourseAsync(CreateCourseDto dto)
        {
            var loggedUserId = _currentUserService.UserId;
            var loggedUserRole = _currentUserService.Role;

            if (loggedUserId == null ||  !string.Equals(loggedUserRole, nameof(Roles.Instructor), StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Unauthorized user ...");

            var instructor = 
                await _instructorRepository.GetInstructorDetailsByUserIdAsync(loggedUserId);

            if (instructor == null)
                throw new UnauthorizedAccessException("User is not a valid instructor ...");

            var newCourse = Course.Create(
                dto.Title,
                dto.Credits,
                dto.EntrollmentLimit
            );

            newCourse.InstructorId = instructor?.Id;
            newCourse.Description = dto.Description;
            newCourse.CategoryEnum = dto.CategoryEnum;
            newCourse.DurationHours = dto.DurationHours;

            await _instructorRepository.CreateNewCourseAsync(newCourse);

        }

        public async Task UploadCourseContentAsync(UploadCourseContentDto dto)
        {
            var loggedUserId = _currentUserService.UserId;
            if (loggedUserId == null)
                throw new UnauthorizedAccessException("Unauthorized user ...");

            var instructor = 
                await _instructorRepository.GetInstructorDetailsByUserIdAsync(loggedUserId);

            if (instructor == null)
                throw new UnauthorizedAccessException("User is not a valid instructor ...");

            var content = CourseContent.Create(
                dto.CourseId,
                dto.Title,
                dto.FileName
            );

            var course = 
                await _instructorRepository.GetCourseDetailsByCourseIdAsync(dto.CourseId);

            if (instructor.Id != course.InstructorId)
                throw new Exception("Not your course ...");

            var folder = Path.Combine("wwwroot", "uploads", content.CourseId.ToString());
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.FileName)}";
            var fullPath = Path.Combine(folder, fileName);

            try
            {
                using var fs = new FileStream(fullPath, FileMode.Create);
                await dto.FileStream.CopyToAsync(fs);

                content.Description = dto.Description;
                content.InstructorId = instructor.Id;
                content.FileUrl = $"/uploads/{dto.CourseId}/{fileName}";
                content.FileSize = fs.Length;
                await _instructorRepository.UploadCourseContentAsync(content);
            }
            catch
            {
                if(File.Exists(fullPath))
                    File.Delete(fullPath);
                throw;
            }
        }
    }
}
