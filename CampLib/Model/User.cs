using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampLib.Model
{
    [Table("Users")]  // 👈 MATCH DIT RIGTIGE TABELLNAVN
    public class User
    {
        [Key]
        [Column("Iduser")]   // 👈 MATCH DIN RIGTIGE PK
        public int Iduser { get; set; }

        private string _email;

        [Column("Email")]     // 👈 MATCH DB kolonnen
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

        public User() { }

        public User(string email)
        {
            Email = email;
        }
    }
}