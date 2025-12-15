using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CampLib.Model;

namespace KlasseLib
{
    public class Issue_Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idcomment { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsInternal { get; set; }

        // FK → Issue
        public int IssueId { get; set; }
        public Issue? Issue { get; set; }

        // FK → User der skrev kommentaren
        public int CreatedByUserId { get; set; }
        public User? CreatedBy { get; set; }
    }
}