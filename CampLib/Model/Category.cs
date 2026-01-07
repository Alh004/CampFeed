using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlasseLib
{
    [Table("Categories")]  // matches your DB table
    public class Category
    {
        [Key]
        [Column("CategoryId")]   // matches your DB PK
        public int CategoryId { get; set; }

        [Column("Name")]
        public string Name { get; set; } = string.Empty;

        [Column("ParentCategoryId")]
        public int? ParentCategoryId { get; set; }

      
    }
}