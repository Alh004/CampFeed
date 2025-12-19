namespace CampApi.DTO
{
    public class ReportDto
    {
        public string Email { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
    }
}