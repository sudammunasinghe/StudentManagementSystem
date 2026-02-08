using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Instructor : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NIC { get; set; }
        public string Address { get; set; }

        private Instructor() { }

        public static void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First Name is required ...");
        }

        public static void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last Name is required ...");
        }

        public static void ValidateEmail(string email)
        {
            if (!email.Contains("@gmail.com"))
                throw new DomainException("Invalid Email Format ...");
        }

        public static void ValidateNIC(string nic)
        {
            if (string.IsNullOrWhiteSpace(nic) || nic.Length < 10)
                throw new DomainException("Invalid NIC ...");
        }

        public static Instructor Create(
            string? firstName,
            string? lastName,
            string? email,
            string? nic,
            string? address)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidateNIC(nic);

            return new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                NIC = nic,
                Address = address
            };
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? email,
            string? nic,
            string? address)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
                ValidateFirstName(firstName);
            if (!string.IsNullOrWhiteSpace(lastName))
                ValidateLastName(lastName);
            if (!string.IsNullOrWhiteSpace(email))
                ValidateEmail(email);
            if (!string.IsNullOrWhiteSpace(nic))
                ValidateNIC(nic);

            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
            Email = email ?? Email;
            NIC = nic ?? NIC;
            Address = address ?? Address;
        }
    }
}
