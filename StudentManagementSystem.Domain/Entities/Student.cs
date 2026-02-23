using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Student : SystemUser
    {
        public double? GPA { get; set; }
        public List<Education>? EducationDetails { get; set; }

        private Student() { }

        public static void ValidateGPA(double? gpa)
        {
            if (gpa > 4.00 || gpa < 0.00)
                throw new DomainException("Invalid GPA ...");
        }

        public static Student Create(
            double? gpa
            )
        {
            return new Student
            {
                GPA = gpa,
                EducationDetails = new List<Education>()
            };
        }

        public void Update(
            double? gpa
            )
        {
            GPA = gpa ?? GPA;
        }

    }
}
