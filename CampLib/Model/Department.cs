using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlasseLib.Model

    //Added by 

{
    [Table("Department")]   // 👈 MATCH DIT RIGTIGE TABELLNAVN
    public class Department
    {
        [Key]
        [Column("Iddepartment")]   // 👈 MATCH DIN RIGTIGE PK
        public int Iddepartment { get; set; }

        private string _name;

        [Column("Name")]      // 👈 MATCH DB kolonnen
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Department name må ikke være tom.");

                _name = value;
            }
        }

        public Department() { }

        public Department(string name)
        {
            Name = name;
        }
    }
}