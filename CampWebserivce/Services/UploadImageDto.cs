using Microsoft.AspNetCore.Http;

namespace CampApi.DTO;
f
public class UploadImageDto
{
    public IFormFile File { get; set; } = null!;
}