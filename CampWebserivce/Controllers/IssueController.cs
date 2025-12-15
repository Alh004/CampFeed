using CampApi.DTO;
using CampWebservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;

namespace CampApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssueController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CloudinaryService _cloudinaryService;

    public IssueController(
        AppDbContext context,
        CloudinaryService cloudinaryService)
    {
        _context = context;
        _cloudinaryService = cloudinaryService;
    }

    // GET: api/issue
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var issues = await _context.Issues.ToListAsync();
        return Ok(issues);
    }

    // GET: api/issue/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var issue = await _context.Issues
            .FirstOrDefaultAsync(i => i.Idissue == id);

        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        return Ok(issue);
    }

    // POST: api/issue
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IssueCreateDto dto)
    {
        if (dto == null)
            return BadRequest("Missing body");

        var issue = new Issue
        {
            Title = dto.Title,
            Description = dto.Description,
            RoomId = dto.RoomId,
            CategoryId = dto.CategoryId,
            ReporterUserId = dto.ReporterUserId,
            Status = "Ny",
            Severity = "Middel",
            CreatedAt = DateTime.UtcNow
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        // Gem billede-URL hvis sendt med
        if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
        {
            var image = new Issue_Image
            {
                IssueId = issue.Idissue,
                FilePath = dto.ImageUrl,
                FileName = Path.GetFileName(dto.ImageUrl),
                ContentType = "image/jpeg",
                UploadedByUserId = dto.ReporterUserId,
                UploadedAt = DateTime.UtcNow
            };

            _context.Issue_Images.Add(image);
            await _context.SaveChangesAsync();
        }

        return Ok(new
        {
            message = "Issue created",
            issueId = issue.Idissue
        });
    }

    // POST: api/issue/upload
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadIssueImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        var result = await _cloudinaryService.UploadImageAsync(
            stream,
            file.FileName
        );

        return Ok(new
        {
            imageUrl = result.SecureUrl.ToString(),
            publicId = result.PublicId
        });
    }

    // PUT: api/issue/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] IssueUpdateStatusDto dto)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        issue.SetStatus(dto.Status);
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(issue);
    }

    // PUT: api/issue/{id}/assign/{userId}
    [HttpPut("{id}/assign/{userId}")]
    public async Task<IActionResult> AssignIssue(int id, int userId)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        issue.AssignedToUserId = userId;
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(issue);
    }
}
