namespace StudentManagementSystem.Domain.Entities
{
    public class Instructor : SystemUser
    {
        public int ExperienceYears { get; set; }
        public decimal? PreferredSalary { get; set; }
        public bool? IsApproved { get; set; }

        private Instructor() { }

        public static Instructor Create(
            string? firstName,
            string? lastName,
            string? address,
            string? contactNumber,
            string? nic,
            int experienceYears,
            decimal? preferredSalary
            )
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateContactNumber(contactNumber);
            var info = ExtractNicInformation(nic);

            return new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                ContactNumber = contactNumber,
                NIC = nic,
                ExperienceYears = experienceYears,
                PreferredSalary = preferredSalary,
                DateOfBirth = info.DateOfBirth,
                Gender = info.Gender
            };
        }

        public void Update(
            string? firstName,
            string? lastName,
            string? nic,
            string? address)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                ValidateFirstName(firstName);
                FirstName = firstName.Trim();
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                ValidateLastName(lastName);
                LastName = lastName.Trim();
            }

            if (address != null)
                Address = address;
        }
    }
}
