using System.ComponentModel.DataAnnotations;

public class Issue_Image
{
    [Key]
    public int ImageId { get; set; }

    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }

    public int IssueId { get; set; }

    public DateTime UploadedAt { get; set; }

    public int UploadedByUserId { get; set; }
}