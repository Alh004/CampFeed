using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;
using CampApi.DTO;
using CampLib.Model;

namespace CampApi.Controllers
{
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

                ImageUrl = dto.ImageUrl,   // ✅ Cloudinary URL
                CategoryId = null,         // ✅ Admin sætter senere
                Status = "Ny",
                Severity = "Middel",

                CreatedAt = DateTime.UtcNow
            };

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            // ✅ 4) Return OK
            return Ok(new
            {
                message = "Sag oprettet",
                issueId = issue.Idissue
            });
        }
    }
}
