using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KlasseLib
{
    public class Issue_Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idimage { get; set; }

        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;

        // FK → Issue
        public int IssueId { get; set; }

        // FK → User
        public int UploadedByUserId { get; set; }
    }
}