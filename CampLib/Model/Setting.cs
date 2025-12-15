using System.ComponentModel.DataAnnotations;

namespace KlasseLib
{
    public class Setting
    {
        [Key]                    // ← EF Core kræver dette
        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public Setting() {}
    }
}