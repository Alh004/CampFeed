namespace CampApi.DTO
{
    public class ReportDto
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RoomId { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
    }
}