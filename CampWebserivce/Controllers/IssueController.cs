using CampApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using CampWebservice.Services; // <-- Hvis din EmailService namespace er anderledes, ret den
using Microsoft.Extensions.Logging;

namespace CampApi.Controllers;

[ApiController]
[Route("api/issue")]
public class IssueController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<IssueController> _logger;

    public IssueController(AppDbContext context, IEmailService emailService, ILogger<IssueController> logger)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    // 🔎 Helper: find reporterens email via ReporterUserId
    private async Task<string?> GetReporterEmailAsync(int? reporterUserId)
    {
        if (reporterUserId == null) return null;

        return await _context.Users
            .Where(u => u.Iduser == reporterUserId.Value)
            .Select(u => u.Email)
            .FirstOrDefaultAsync();
    }

    // GET: api/issue (ADMIN)
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
                imageUrl = i.ImageUrl
            })
            .ToListAsync();

        return Ok(issues);
    }

    // GET: api/issue/{id}
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

    // PUT: api/issue/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueUpdateStatusDto dto)
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

        var shouldSendClosedEmail = false;

        // 🔥 Luk / genåbn logik
        if (dto.Status == "Lukket")
        {
            if (issue.ClosedAt == null)
            {
                issue.ClosedAt = DateTime.UtcNow;
                shouldSendClosedEmail = true;
            }
        }
        else
        {
            issue.ClosedAt = null;
        }

        await _context.SaveChangesAsync();

        // ✉️ Send mail efter DB er opdateret
        if (shouldSendClosedEmail && issue.ReporterUserId != null)
        {
            try
            {
                var email = await GetReporterEmailAsync(issue.ReporterUserId);
                if (!string.IsNullOrWhiteSpace(email))
                {
                    await _emailService.SendIssueClosedAsync(email, issue.Idissue, issue.Title);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send closed email for IssueId {IssueId}", issue.Idissue);
            }
        }

        return Ok(new
        {
            message = "Issue updated",
            issueId = issue.Idissue,
            status = issue.Status,
            closedAt = issue.ClosedAt
        });
    }

    // PUT: api/issue/{id}/category
    [HttpPut("{id}/category")]
    public async Task<IActionResult> AssignCategory(int id, [FromBody] IssueUpdateStatusDto dto)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound(new { message = "Issue not found" });

        issue.CategoryId = dto.CategoryId;
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Category assigned" });
    }

    // PUT: api/issue/{id}/assign/{userId}
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
 