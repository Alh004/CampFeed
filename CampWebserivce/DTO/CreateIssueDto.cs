using KlasseLib;

namespace CampApi.DTO
{
    public class IssueCreateDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int RoomId { get; set; }
        public int CategoryId { get; set; }
        public int ReporterId { get; set; }
        public string Severity { get; set; }
    }
}