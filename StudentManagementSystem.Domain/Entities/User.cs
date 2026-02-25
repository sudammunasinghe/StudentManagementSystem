using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? NIC { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int? RoleId { get; set; }
        public string PasswrodResetToken { get; set; }
        public DateTime PasswrodResetTokenExpiry { get; set; }
        private User() { }

        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email is required ...");
        }

        public static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password is required ...");
        }

        public static void ValidateFirstName(string? firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First Name is required ...");
        }

        public static void ValidateLastName(string? lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last Name is required ...");
        }

        public static void ValidateContactNumber(string? contactNumber)
        {
            if (string.IsNullOrWhiteSpace(contactNumber))
                throw new DomainException("Contact Number is required ...");
        }

        public static NicInfo ExtractNicInformation(string nic)
        {
            if (string.IsNullOrWhiteSpace(nic))
                throw new DomainException("NIC is required ...");

            int year;
            int dayOfYear;
            char gender;

            if (nic.Length == 10)
            {
                dayOfYear = int.Parse(nic.Substring(2, 3));
                year = 1900 + int.Parse(nic.Substring(0, 2));
            }
            else if (nic.Length == 12)
            {
                dayOfYear = int.Parse(nic.Substring(4, 3));
                year = int.Parse(nic.Substring(0, 4));
            }
            else
            {
                throw new DomainException("Invalid NIC format ...");
            }

            if (dayOfYear > 500)
            {
                gender = 'F';
                dayOfYear -= 500;
            }
            else
            {
                gender = 'M';
            }
            var dateOfBirth = new DateTime(year, 1, 1).AddDays(dayOfYear - 1);

            return new NicInfo
            {
                DateOfBirth = dateOfBirth,
                Gender = gender
            };
        }

        public static User Create(
            string? firstName,
            string? lastName,
            string? address,
            string? nic,
            string? contactNumber,
            string? email,
            string? password,
            int? roleId
            )
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateContactNumber(contactNumber);
            ValidateEmail(email);
            ValidatePassword(password);
            var info = ExtractNicInformation(nic);


            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                NIC = nic,
                ContactNumber = contactNumber,
                Email = email,
                RoleId = roleId,
                DateOfBirth = info.DateOfBirth,
                Gender = info.Gender
            };
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? address,
            string? nic,
            string? contactNumber,
            string? email
            )
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateContactNumber(contactNumber);
            ValidateEmail(email);
            var info = ExtractNicInformation(nic);

            FirstName = firstName;
            LastName = lastName;
            Address = address;
            ContactNumber = contactNumber;
            Email = email;
            DateOfBirth = info.DateOfBirth;
            Gender = info.Gender;
        }
    }
}
