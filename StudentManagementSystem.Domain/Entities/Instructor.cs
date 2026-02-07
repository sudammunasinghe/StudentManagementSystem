using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Instructor Create(
            string firstName,
            string lastName,
            string email,
            string nic,
            string address)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new Exception("First Name is required ...");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new Exception("Last Name is required ...");

            if (!email.Contains("@gmail.com"))
                throw new Exception("Invalid Email Format ...");

            if (string.IsNullOrWhiteSpace(nic) || nic.Length < 10)
                throw new Exception("Invalid NIC ...");

            return new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                NIC = nic,
                Address = address
            };
        }
    }
}
