using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Credits { get; set; }
        public string? Status { get; set; }
        public int? InstructorId { get; set; }

        private Course() { }

        public static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required ...");
        }

        public static void validateInstructorId(int? instructorId)
        {
            if (string.IsNullOrWhiteSpace(instructorId.ToString()))
                throw new DomainException("Instructor Id is required ...");
        }

        public static void validateCredits(int? credits)
        {
            if (string.IsNullOrWhiteSpace(credits.ToString()))
                throw new DomainException("Credit is required ...");

            if (credits > 50 || credits < 10)
                throw new DomainException("Invalid Credit range ...");
        }

        public static Course Create(string? title, int? credits, int? instructorId)
        {
            ValidateTitle(title);
            validateInstructorId(instructorId);
            validateCredits(credits);

            return new Course
            {
                Title = title,
                Credits = credits,
                InstructorId = instructorId
            };
        }


        public void Update(string? title, int? credits, int? instructorId)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                ValidateTitle(title);
                Title = title;
            }
            if (!string.IsNullOrWhiteSpace(instructorId.ToString()))
            {
                validateInstructorId(instructorId);
                InstructorId = instructorId;
            }
            if (!string.IsNullOrWhiteSpace(credits.ToString()))
            {
                validateCredits(credits);
                Credits = credits;
            }
        }

    }
}
