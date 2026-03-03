using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class CourseContent : BaseEntity
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? InstructorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ContentType { get; set; }
        public string? FileUrl { get; set; }
        public long? FileSize { get; set; }

        private CourseContent() { }

        public static void ValidateCourseId(int? courseId)
        {
            if (string.IsNullOrWhiteSpace(courseId.ToString()))
                throw new DomainException("Course Id is required ...");
        }

        public static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required ...");
        }

        public static void ValidateContentType(string? contentType)
        {
            var allowedExtensuions = new[] { ".pdf", ".mp4", ".jpg", ".png" };
            if (!allowedExtensuions.Contains(contentType))
                throw new DomainException("Invalid File Type ...");

        }

        public static CourseContent Create(int? courseId, string? title, string? fileName)
        {
            var contentType = Path.GetExtension(fileName);
            ValidateCourseId(courseId);
            ValidateTitle(title);
            ValidateContentType(contentType);

            return new CourseContent
            {
                CourseId = courseId,
                Title = title,
                ContentType = contentType
            };
        }

        public void Update(string? title, string? description)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                ValidateTitle(title);
                Title = title;
            }

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;
        }
    }
}
