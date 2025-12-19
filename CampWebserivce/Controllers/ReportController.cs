using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using CampApi.DTO;
using CampLib.Model;

namespace CampApi.Controllers;

[ApiController]
[Route("api/report")]
public class ReportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReportDto dto)
    {
        // 1️⃣ Valider input
        if (dto == null)
            return BadRequest("Request mangler");

        // 2️⃣ Find eller opret user
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            user = new User
            {
                Email = dto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // 3️⃣ Opret Issue
        var issue = new Issue
        {
            Title = dto.Title,
            Description = dto.Description,
            RoomId = dto.RoomId,
            CategoryId = dto.CategoryId,
            ReporterUserId = user.Iduser,
            Status = "Ny",
            Severity = "Middel",
            CreatedAt = DateTime.UtcNow
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync(); // 🔑 GIVER issue.Idissue

        // 4️⃣ Gem billede (hvis findes)
        if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
        {
            var image = new Issue_Image
            {
                IssueId = issue.Idissue,
                FilePath = dto.ImageUrl,
                FileName = Path.GetFileName(dto.ImageUrl),
                ContentType = "image/jpeg",
                UploadedByUserId = user.Iduser,
                UploadedAt = DateTime.UtcNow
            };

            _context.Issue_Images.Add(image);
            await _context.SaveChangesAsync();
        }

        // 5️⃣ Return OK
        return Ok(new
        {
            message = "Sag oprettet",
            issueId = issue.Idissue
        });
    }
}