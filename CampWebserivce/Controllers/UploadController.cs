using CampApi.DTO;
using CampWebservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampApi.Controllers;

[ApiController]
[Route("api/upload")]
public class UploadController : ControllerBase
{
    private readonly CloudinaryService _cloudinary;

    public UploadController(CloudinaryService cloudinary)
    {
        _cloudinary = cloudinary;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadImageDto dto)
    {
        if (dto == null)
            return BadRequest("Form-data mangler");

        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("Ingen fil uploadet");

        using var stream = dto.File.OpenReadStream();
        var result = await _cloudinary.UploadImageAsync(stream, dto.File.FileName);

        return Ok(new
        {
            imageUrl = result.SecureUrl.ToString()
        });
    }
} //Added by 
