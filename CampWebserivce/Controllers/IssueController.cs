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

    // GET: api/issue (admin)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var issues = await _context.Issues
            .Include(i => i.Reporter)
            .Include(i => i.Room)
            .Include(i => i.Category)
            .ToListAsync();

        return Ok(issues);
    }

    // GET: api/issue/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var issue = await _context.Issues
            .Include(i => i.Reporter)
            .Include(i => i.Room)
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Idissue == id);

        if (issue == null)
            return NotFound();

        return Ok(issue);
    }

    // PUT: api/issue/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] IssueUpdateStatusDto dto)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound();

        issue.SetStatus(dto.Status);
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(issue);
    }

    // PUT: api/issue/{id}/assign/{userId}
    [HttpPut("{id}/assign/{userId}")]
    public async Task<IActionResult> Assign(int id, int userId)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound();

        issue.AssignedToUserId = userId;
        issue.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(issue);
    }
}