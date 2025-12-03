using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampLib.Model
{
    public class User
    {

        // Instansfelter
        private int _id;
        private string _name;
        private string _email;
        private string _password;
        private string _phone;

        // property
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email
        {
            get => _email;
            set
            {
                if (!value.EndsWith("@edu.zealand.dk"))
                    throw new ArgumentException("Email skal ende på '@edu.zealand.dk' for at kunne logge ind.");

                _email = value;
            }
        }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        // Constructor
        public User(int id, string name, string email, string password, bool isActive)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            IsActive = isActive;
        }

        // ToString
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Email: {Email}, IsActive: {IsActive}";
        }

    }
}
