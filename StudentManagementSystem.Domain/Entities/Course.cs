using StudentManagementSystem.Domain.Exceptions;
using System.Data;

namespace StudentManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Credits { get; set; }
        public string? Status { get; set; }

        private Course() { }

        public static void ValidateTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title is required ...");
        }

        public static void validateCredits(int? credits)
        {
            if (string.IsNullOrWhiteSpace(credits.ToString()))
                throw new DomainException("Credit is required ...");

            if (credits > 50 || credits < 10)
                throw new DomainException("Invalid Credit range ...");

        }

        public static Course Create(string? title, int? credits)
        {
            ValidateTitle(title);
            validateCredits(credits);

            return new Course
            {
                Title = title,
                Credits = credits
            };
        }


        public void Update(string? title, int? credits)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                ValidateTitle(title);
                Title = title;
            }
            if (!string.IsNullOrWhiteSpace(credits.ToString()))
            {
                validateCredits(credits);
                Credits = credits;
            }
        }
        
    }
}
