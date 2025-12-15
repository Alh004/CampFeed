using CampWebservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampApi.Controllers;

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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(IFormFile file, int issueId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil uploadet.");

        using var stream = file.OpenReadStream();
        var result = await _cloudinary.UploadImageAsync(
            stream,
            file.FileName
        );

        return Ok(new
        {
            imageUrl = result.SecureUrl.ToString(),
            publicId = result.PublicId
        });
    }
}