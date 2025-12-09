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
       
        private string _email;
     
       

        // property
        public int Id { get; set; }
       
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
       
       

        // Constructor
        public User(int id, string email)
        {
            Id = id;
            Email = email;

        }

        // ToString
        public override string ToString()
        {
            return $"Id: {Id}, Email: {Email}";
        }

    }
}
