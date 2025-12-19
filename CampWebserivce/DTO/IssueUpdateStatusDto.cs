public class IsseuUpdateStatusDto
{
    public string Status { get; set; } = string.Empty;   // Ny / I gang / Lukket
    public string Severity { get; set; } = string.Empty; // Lav / Middel / Høj
    public int CategoryId { get; set; }
}