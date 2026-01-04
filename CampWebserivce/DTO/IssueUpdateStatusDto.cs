namespace CampApi.DTO
{
    public class IssueUpdateStatusDto
    {
    public string Status { get; set; }   // Ny / I gang / Lukket
    public string Severity { get; set; } // Lav / Middel / Høj
    public int CategoryId { get; set; }
    
    }

}