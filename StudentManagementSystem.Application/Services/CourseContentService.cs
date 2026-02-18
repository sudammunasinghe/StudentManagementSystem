using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;
using StudentManagementSystem.Domain.Entities;

namespace StudentManagementSystem.Application.Services
{
    public class CourseContentService : ICourseContentService
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        public CourseContentService(ICourseContentRepository courseContentRepository, ICourseRepository courseRepository)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
        }

        public async Task UploadCourseContentAsync(UploadCourseContentDto dto)
        {
            var content = CourseContent.Create(
                    dto.CourseId,
                    dto.InstructorId,
                    dto.Title,
                    dto.FileName
                );

            var course =
                await _courseRepository.GetCourseDetailsByCourseIdAsync(content.CourseId);

            if (course == null)
                throw new NotFoundException("Course not found ...");

            if (content.InstructorId != course.InstructorId)
                throw new UnauthorizedAccessException("Not your course ...");

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
                content.FileUrl = $"/uploads/{dto.CourseId}/{fileName}";
                content.FileSize = fs.Length;
                await _courseContentRepository.UploadCourseContentAsync(content);
            }
            catch
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
                throw;
            }
        }

        public async Task UpdateCourseMetaDataAsync(int contentId, UpdateCourseMetaDataDto dto)
        {
            var courseContent =
                await _courseContentRepository.GetCourseContentByCourseContentIdAsync(contentId);

            if (courseContent == null)
                throw new NotFoundException("Course content not found ...");

            courseContent.Update(
                dto.Title,
                dto.Description
            );

            var affectedRows =
                await _courseContentRepository.UpdateCourseMetaDataAsync(courseContent);

            if (affectedRows == 0)
                throw new Exception("Course content update failed ...");
        }

        public async Task InactivateCourseContentByCourseContentIdAsync(int? contentId)
        {
            var courseContent =
                await _courseContentRepository.GetCourseContentByCourseContentIdAsync(contentId);

            if (courseContent == null)
                throw new NotFoundException("Course content not found ...");

            var affectedRows =
                await _courseContentRepository.InactivateCourseContentByCourseContentIdAsync(contentId);

            if (affectedRows == 0)
                throw new Exception("Course content inactivation failed ...");
        }
    }
}
