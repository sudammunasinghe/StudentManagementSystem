using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Student : SystemUser
    {
        public double? GPA { get; set; }

        private Student() { }

        public static void ValidateGPA(double? gpa)
        {
            if (gpa > 4.00 || gpa < 0.00)
                throw new DomainException("Invalid GPA ...");
        }

        public static Student Create(
            string? firstName,
            string? lastName,
            string? address,
            string? contactNumber,
            string? nic,
            double? gpa
            )
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateContactNumber(contactNumber);
            ValidateGPA(gpa);
            var info = ExtractNicInformation(nic);


            return new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                ContactNumber = contactNumber,
                NIC = nic,
                GPA = gpa,
                Gender = info.Gender,
                DateOfBirth = info.DateOfBirth
            };
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? address,
            string? nic,
            double? gpa
            )
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                ValidateFirstName(firstName);

            if (!string.IsNullOrWhiteSpace(lastName))
                ValidateLastName(lastName);


            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            Address = address ?? Address;
            NIC = nic ?? NIC;
        }

    }
}
