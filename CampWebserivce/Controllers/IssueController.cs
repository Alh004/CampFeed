using CampApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;

namespace CampApi.Controllers;

[ApiController]
[Route("api/issue")]
public class IssueController : ControllerBase
{
    private readonly AppDbContext _context;

    public IssueController(AppDbContext context)
    {
        _context = context;
    }

    // =========================
    // GET: api/issue (ADMIN)
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var issues = await _context.Issues
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new
            {
                idissue = i.Idissue,
                title = i.Title,
                description = i.Description,
                status = i.Status,
                severity = i.Severity,
                roomId = i.RoomId,
                categoryId = i.CategoryId,
                reporterUserId = i.ReporterUserId,
                assignedToUserId = i.AssignedToUserId,
                createdAt = i.CreatedAt,
                lastUpdatedAt = i.LastUpdatedAt,
                closedAt = i.ClosedAt,

                // ✅ BILLEDE LÆSES DIREKTE FRA ISSUES
                imageUrl = i.ImageUrl
            })
            .ToListAsync();

        return Ok(issues);
    }

    // =========================
    // GET: api/issue/{id}
    // =========================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var issue = await _context.Issues
            .Where(i => i.Idissue == id)
            .Select(i => new
            {
                idissue = i.Idissue,
                title = i.Title,
                description = i.Description,
                status = i.Status,
                severity = i.Severity,
                roomId = i.RoomId,
                categoryId = i.CategoryId,
                reporterUserId = i.ReporterUserId,
                assignedToUserId = i.AssignedToUserId,
                createdAt = i.CreatedAt,
                lastUpdatedAt = i.LastUpdatedAt,
                closedAt = i.ClosedAt,
                imageUrl = i.ImageUrl
            })
            .FirstOrDefaultAsync();

        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        return Ok(issue);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIssue(
        int id,
        [FromBody] IsseuUpdateStatusDto dto)
    {
        if (dto == null)
            return BadRequest("Request body mangler");

        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        // 🔒 Opdater felter
        issue.Status = dto.Status;
        issue.Severity = dto.Severity;
        issue.CategoryId = dto.CategoryId;
        issue.LastUpdatedAt = DateTime.UtcNow;

        // 🔥 Luk / genåbn logik
        if (dto.Status == "Lukket")
        {
            // Sæt ClosedAt kun én gang
            if (issue.ClosedAt == null)
                issue.ClosedAt = DateTime.UtcNow;
        }
        else
        {
            // Hvis sagen genåbnes → nulstil ClosedAt
            issue.ClosedAt = null;
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Issue updated",
            issueId = issue.Idissue,
            status = issue.Status,
            closedAt = issue.ClosedAt
        });
    }


    // =========================
    // PUT: api/issue/{id}/category
    // =========================
    [HttpPut("{id}/category")]
    public async Task<IActionResult> AssignCategory(
        int id,
        [FromBody] IsseuUpdateStatusDto dto)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        issue.CategoryId = dto.CategoryId;
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Category assigned" });
    }

    // =========================
    // PUT: api/issue/{id}/assign/{userId}
    // =========================
    [HttpPut("{id}/assign/{userId}")]
    public async Task<IActionResult> Assign(int id, int userId)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        issue.AssignedToUserId = userId;
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Issue assigned" });
    }
}
