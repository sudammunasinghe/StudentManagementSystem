using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }
        public int? InstructorId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CategoryEnum { get; set; }
        public int? Credits { get; set; }
        public int? DurationHours { get; set; }
        public int? EntrollmentLimit { get; set; }

        private Course() { }

        public static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required ...");
        }

        public static void ValidateCredits(int? credits)
        {
            if (string.IsNullOrWhiteSpace(credits.ToString()))
                throw new DomainException("Credit is required ...");

            if (credits > 50 || credits < 10)
                throw new DomainException("Invalid Credit range ...");
        }

        public static void ValidateEnrollmentLimit(int? limit)
        {
            if (string.IsNullOrWhiteSpace(limit.ToString()))
                throw new DomainException("Enrollment limit is required ...");
        }

        public static Course Create(string? title, int? credits, int? enrollmentLimit)
        {
            ValidateTitle(title);
            ValidateCredits(credits);
            ValidateEnrollmentLimit(enrollmentLimit);

            return new Course
            {
                Title = title,
                Credits = credits,
                EntrollmentLimit = enrollmentLimit
            };
        }


        public void Update(string? title, int? credits, int? enrollmentLimit)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                ValidateTitle(title);
                Title = title;
            }
            if (!string.IsNullOrWhiteSpace(credits.ToString()))
            {
                ValidateCredits(credits);
                Credits = credits;
            }
            if (!string.IsNullOrWhiteSpace(EntrollmentLimit.ToString()))
            {
                ValidateEnrollmentLimit(enrollmentLimit);
                EntrollmentLimit = enrollmentLimit;
            }
        }

    }
}
