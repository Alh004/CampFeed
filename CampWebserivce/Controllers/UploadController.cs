using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly CloudinaryService _cloudinary;

    public UploadController(CloudinaryService cloudinary)
    {
        _cloudinary = cloudinary;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int issueId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil uploadet.");

        var result = await _cloudinary.UploadAsync(file, folder: $"issues/{issueId}");

        // Her kan du også gemme i databasen, hvis du vil
        // f.eks. INSERT INTO Issue_Image (FilePath, FileName, ContentType, IssueId) ...

        return Ok(new { url = result.SecureUrl.ToString(), publicId = result.PublicId });
    }
}