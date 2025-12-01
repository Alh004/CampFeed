namespace KlasseLib;

public class Issue_Comment
{
    public int Id { get; set; }
    public string Text { get; set; }
    DateTime createdAt;
    public int IssueID { get; set; }
    public int CreatedByUserID { get; set; }

    public Issue_Comment(DateTime createdAt, int id, string text, int issueId, int createdByUserId)
    {
        this.createdAt = createdAt;
        Id = id;
        Text = text;
        IssueID = issueId;
        CreatedByUserID = createdByUserId;
    }


    public override string ToString()
    {
        return
            $"{nameof(createdAt)}: {createdAt}, {nameof(Id)}: {Id}, {nameof(Text)}: {Text}, {nameof(IssueID)}: {IssueID}, {nameof(CreatedByUserID)}: {CreatedByUserID}";
    }
}