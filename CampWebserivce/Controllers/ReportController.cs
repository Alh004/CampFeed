using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using CampApi.DTO;
using CampLib.Model;
using CampWebservice.Services; // <-- ret namespace hvis din service ligger et andet sted

namespace CampApi.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(AppDbContext context, IEmailService emailService, ILogger<ReportController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
        }

        // ---------------------------------------------------------
        // PUT: Opdater Issue (bemærk: din route her er nok forkert, se note nederst)
        // ---------------------------------------------------------
        [HttpPut("api/issue/{id}")]
        public async Task<IActionResult> UpdateIssue(int id, IssueUpdateStatusDto dto)
        {
            var issue = await _context.Issues.FindAsync(id);

            if (issue == null)
                return NotFound();

            issue.Status = dto.Status;
            issue.Severity = dto.Severity;
            issue.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            return Ok(issue);
        }

        // ---------------------------------------------------------
        // POST: Opret Report + Issue + send bekræftelsesmail
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReportDto dto)
        {
            // 🔒 1) Valider input
            if (dto == null)
                return BadRequest(new { message = "Request mangler" });

            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Title) ||
                string.IsNullOrWhiteSpace(dto.Description))
            {
                return BadRequest(new { message = "Manglende felter" });
            }

            // 👤 2) Find eller opret bruger
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

            // 📝 3) Opret Issue
            var issue = new Issue
            {
                Title = dto.Title,
                Description = dto.Description,
                RoomId = dto.RoomId,
                ReporterUserId = user.Iduser,

                ImageUrl = dto.ImageUrl,
                CategoryId = null, // Admin sætter senere
                Status = "Ny",
                Severity = "Middel",
                CreatedAt = DateTime.UtcNow
            };

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            // ✉️ 4) Send bekræftelsesmail (må ikke blokere oprettelsen)
            try
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    await _emailService.SendIssueCreatedAsync(user.Email, issue.Idissue, issue.Title);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send created email for IssueId {IssueId}", issue.Idissue);
                // fortsæt – issue er allerede oprettet
            }
            //Added by 

            // ✅ 5) Return OK
            return Ok(new
            {
                message = "Sag oprettet",
                issueId = issue.Idissue
            });
        }
    }
}
