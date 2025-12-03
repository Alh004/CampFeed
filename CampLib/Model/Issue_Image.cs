namespace KlasseLib;

public class Issue_Image
{
    public int ImageId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }    
    public string ContentType { get; set; }
    public DateTime UploadDate { get; set; }
    public DateTime IssueDate { get; set; }
}