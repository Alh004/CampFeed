namespace KlasseLib;

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
    public int Id { get; set; }

    // Grunddata
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Tidsstempler
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    // Status / alvorlighed
    public IssueStatus Status { get; set; }
    public IssueSeverity Severity { get; set; }


    // Relationer til andre entiteter (FKs)
    public int RoomId { get; set; }
    public int CategoryId { get; set; }
    public int? AssignedDepartmentId { get; set; }
    public int? AssignedToUserId { get; set; } // tekniker/medarbejder

    // Default constructor
    public Issue()
    {
        CreatedAt = DateTime.Now;
        Status = IssueStatus.Ny;
        Severity = IssueSeverity.Middel;
    }

    // Hjælpe-konstruktor til oprettelse fra formular
    public Issue(string title, string description, int roomId, int categoryId)
        : this()
    {
        Title = title;
        Description = description;
        RoomId = roomId;
        CategoryId = categoryId;

    }

    public void SetStatus(IssueStatus newStatus)
    {
        Status = newStatus;
        LastUpdatedAt = DateTime.Now;

        if (newStatus == IssueStatus.Løst)
        {
            ClosedAt = DateTime.Now;
        }
    }

    public override string ToString()
    {
        return $"Issue Id={Id}, Title={Title}, Status={Status}, Severity={Severity}, RoomId={RoomId}, CategoryId={CategoryId}, CreatedAt={CreatedAt}";
    }
}
