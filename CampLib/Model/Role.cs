using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlasseLib 
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idrole { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public Role() {}

        public Role(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Role: {Idrole} - {Name}";
        }
    }
}