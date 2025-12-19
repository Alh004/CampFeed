using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using CampApi.DTO;
using CampLib.Model;

namespace CampApi.Controllers;[ApiController]
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
        if (dto == null)
            return BadRequest("Request mangler");

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            user = new User { Email = dto.Email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        var issue = new Issue
        {
            Title = dto.Title,
            Description = dto.Description,
            RoomId = dto.RoomId,
            CategoryId = dto.CategoryId,
            ReporterUserId = user.Iduser,
            ImageUrl = dto.ImageUrl, // 🔥 VIGTIG
            Status = "Ny",
            Severity = "Middel",
            CreatedAt = DateTime.UtcNow
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            issueId = issue.Idissue
        });
    }
}
