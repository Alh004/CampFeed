public class IssueCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int RoomId { get; set; }
    public int CategoryId { get; set; }
    public int ReporterUserId { get; set; }
    public string? ImageUrl { get; set; }  // <-- MATCHER FRONTEND
}