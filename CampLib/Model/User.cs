using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampLib.Model
{
    [Table("Users")]  
    public class User
    {
        [Key]
        [Column("Iduser")]   
        public int Iduser { get; set; }

        private string _email;

        [Column("Email")]     
        public string Email
        {
            get => _email;
            set
            {
                if (!value.EndsWith("@edu.zealand.dk"))
                    throw new ArgumentException("Email skal ende på '@edu.zealand.dk'.");
                _email = value;
            }
        }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User() {}

        public User(string email)
        {
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }
    }
}