using CampApi.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;

namespace CampApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssueController : ControllerBase
{
    private readonly AppDbContext _context;

    public IssueController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/issue
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var issues = await _context.Issues
            .Include(i => i.Room)
            .Include(i => i.Reporter)
            .Include(i => i.Category)
            .ToListAsync();

        return Ok(issues);
    }

    // GET: api/issue/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var issue = await _context.Issues
            .Include(i => i.Room)
            .Include(i => i.Reporter)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (issue == null)
            return NotFound();

        return Ok(issue);
    }

    // POST: api/issue
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IssueCreateDto dto)
    {
        var issue = new Issue
        {
            Title = dto.Title,
            Description = dto.Description,
            RoomId = dto.RoomId,
            CategoryId = dto.CategoryId,
            ReporterUserId = dto.ReporterId,
            Severity = dto.Severity,
            CreatedAt = DateTime.UtcNow
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
    }

    // PUT: api/issue/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] IssueUpdateStatusDto dto)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound();

        issue.SetStatus(dto.Status);
        await _context.SaveChangesAsync();

        return Ok(issue);
    }

    // PUT: api/issue/{id}/assign/{userId}
    [HttpPut("{id}/assign/{userId}")]
    public async Task<IActionResult> AssignIssue(int id, int userId)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue == null)
            return NotFound();

        issue.AssignedToUserId = userId;
        await _context.SaveChangesAsync();

        return Ok(issue);
    }
}
