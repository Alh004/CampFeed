using Microsoft.AspNetCore.Http;

namespace CampApi.DTO;

public class UploadImageDto
{
    public IFormFile File { get; set; } = null!;
}