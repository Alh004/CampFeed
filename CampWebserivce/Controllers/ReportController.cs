using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using CampApi.DTO;
using CampLib.Model;

namespace CampApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] ReportDto dto)
    {
        // 1. FIND USER ELLER OPRET NY
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            user = new User { Email = dto.Email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // 2. OPRET ISSUE
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
        await _context.SaveChangesAsync();

        // 3. GEM BILLEDE HVIS ET FANDTES
        if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
        {
            var image = new Issue_Image
            {
                IssueId = issue.Idissue,
                FilePath = dto.ImageUrl,
                FileName = "",
                ContentType = "",
                UploadedAt = DateTime.UtcNow,
                UploadedByUserId = user.Iduser
            };

            _context.Issue_Images.Add(image);
            await _context.SaveChangesAsync();
        }

        // 4. RETURNER BEKRÆFTELSE
        return Ok(new
        {
            message = "Indberetning modtaget",
            issueId = issue.Idissue
        });
    }
}