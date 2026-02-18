using StudentManagementSystem.Domain.Exceptions;

namespace StudentManagementSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? NIC { get; set; }

        private Student() { }

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

        public static void ValidateEmail(string? email)
        {
            if (!email.Contains("@"))
                throw new DomainException("Invlaid Email ...");
        }

        public static void ValidateNIC(string? nic)
        {
            if (string.IsNullOrWhiteSpace(nic) || nic.Length < 10)
                throw new DomainException("Invlaid NIC ...");
        }

        public static Student Create(
            string? firstName,
            string? lastName,
            string? address,
            string? email,
            string? nic
            )
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidateNIC(nic);

            return new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                Email = email,
                NIC = nic
            };
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? address,
            string? email,
            string? nic)
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
            Address = address ?? Address;
            Email = email ?? Email;
            NIC = nic ?? NIC;
        }

    }
}
