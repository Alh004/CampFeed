using KlasseLib;

namespace WebApplication1.DTO;

public class IsseuDtoCreate
{
    public class IssueCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public int CategoryId { get; set; }
        public int ReporterId { get; set; }
        public IssueSeverity Severity { get; set; }  // ENUM – ikke string
    }

}