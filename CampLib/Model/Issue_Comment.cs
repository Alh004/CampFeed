namespace KlasseLib;

public class Issue_Comment
{
    public int id { get; set; }
    public string Text { get; set; }
    public DateTime createdAt;
    public int Issueid { get; set; }
    public int CreatedByUserid { get; set; }

    public Issue_Comment(DateTime createdAt, int id, string text, int issueId, int createdByUserId)
    {
        this.createdAt = createdAt;
        id = id;
        Text = text;
        Issueid = issueId;
        CreatedByUserid = createdByUserId;
    }


    public override string ToString()
    {
        return
            $"{nameof(createdAt)}: {createdAt}, {nameof(id)}: {id}, {nameof(Text)}: {Text}, {nameof(Issueid)}: {Issueid}, {nameof(CreatedByUserid)}: {CreatedByUserid}";
    }
}