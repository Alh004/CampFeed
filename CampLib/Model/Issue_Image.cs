namespace KlasseLib;

public class Issue_Image
{
    public int ImageId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }    
    public string ContentType { get; set; }
    public DateTime UploadDate { get; set; }
    public int IssueId { get; set; }
    public int UploadedByUserId { get; set; }

    public Issue_Image(int imageId, string filePath, string fileName, string contentType, DateTime uploadDate, int issueId, int uploadedByUserId)
    {
        ImageId = imageId;
        FilePath = filePath;
        FileName = fileName;
        ContentType = contentType;
        UploadDate = uploadDate;
        IssueId = issueId;
        UploadedByUserId = uploadedByUserId;
    }

    public override string ToString()
    {
        return
            $"{nameof(ImageId)}: {ImageId}, {nameof(FilePath)}: {FilePath}, {nameof(FileName)}: {FileName}, {nameof(ContentType)}: {ContentType}, {nameof(UploadDate)}: {UploadDate}, {nameof(IssueId)}: {IssueId}, {nameof(UploadedByUserId)}: {UploadedByUserId}";
    }
}

