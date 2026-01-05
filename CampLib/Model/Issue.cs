using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CampLib.Model;
using KlasseLib.Model;

namespace KlasseLib
{ f
    public enum IssueStatus
    {
        Ny,
        I_gang,
        Afventer,
        Løst
    }

    public enum IssueSeverity
    {
        Lav,
        Middel,
        Høj
    }

    public class Issue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Idissue { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        
        public string? ImageUrl { get; set; }


        // gemmes som tekst
        public string Status { get; set; } = "Ny";
        public string Severity { get; set; } = "Middel";

        [NotMapped]
        public IssueStatus StatusEnum
        {
            get => Enum.Parse<IssueStatus>(Status.Replace(" ", "_"));
            set => Status = value.ToString().Replace("_", " ");
        }

        [NotMapped]
        public IssueSeverity SeverityEnum
        {
            get => Enum.Parse<IssueSeverity>(Severity);
            set => Severity = value.ToString();
        }

        // FK + Navigation
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int ReporterUserId { get; set; }
        public User? Reporter { get; set; }

        public int? AssignedToUserId { get; set; }
        public User? AssignedTo { get; set; }

        public int? AssignedDepartmentId { get; set; }
        public Department? AssignedDepartment { get; set; }

        public void SetStatus(IssueStatus newStatus)
        {
            Status = newStatus.ToString().Replace("_", " ");
            LastUpdatedAt = DateTime.UtcNow;

            if (newStatus == IssueStatus.Løst)
                ClosedAt = DateTime.UtcNow;
        }
    }
}
