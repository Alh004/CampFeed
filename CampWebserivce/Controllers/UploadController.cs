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
    [Consumes("multipart/form-data")] // vigtigt for Swagger
    public async Task<IActionResult> Upload(IFormFile file, int issueId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil uploadet.");

        var result = await _cloudinary.UploadAsync(file, folder: $"issues/{issueId}");

        return Ok(new
        {
            url = result.SecureUrl.ToString(),
            publicId = result.PublicId
        });
    }
}