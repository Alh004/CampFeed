using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CampLib.Model;

namespace KlasseLib
{
    public enum IssueStatus
    {
        Ny,
        Igang,
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
        public int Id { get; set; }

        // Grunddata
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        // Tidsstempler
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Status / alvorlighed
        public IssueStatus Status { get; set; } = IssueStatus.Ny;
        public IssueSeverity Severity { get; set; } = IssueSeverity.Middel;

        // Relationer → Navigation properties + FK

        // 🔥 Room
        public int RoomId { get; set; }
        public Room? Room { get; set; }

        // 🔥 Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // 🔥 Reporter (den studerende der opretter issue)
        public int ReporterUserId { get; set; }
        public User? Reporter { get; set; }

        // 🔥 Assigned-to (tekniker)
        public int? AssignedToUserId { get; set; }
        public User? AssignedTo { get; set; }

        // 🔥 Department (hvis du vil bruge det)
        public int? AssignedDepartmentId { get; set; }
        public Department? AssignedDepartment { get; set; }

        // Constructors
        public Issue() {}

        public Issue(string title, string description, int roomId, int categoryId, int reporterUserId)
        {
            Title = title;
            Description = description;
            RoomId = roomId;
            CategoryId = categoryId;
            ReporterUserId = reporterUserId;

            CreatedAt = DateTime.UtcNow;
        }

        public void SetStatus(IssueStatus newStatus)
        {
            Status = newStatus;
            LastUpdatedAt = DateTime.UtcNow;

            if (newStatus == IssueStatus.Løst)
                ClosedAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return $"Issue Id={Id}, Title={Title}, Status={Status}, Severity={Severity}, RoomId={RoomId}, CategoryId={CategoryId}, CreatedAt={CreatedAt}";
        }
    }
}
