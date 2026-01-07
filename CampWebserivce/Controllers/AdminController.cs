using CampLib.Repository;
using CampLib.Model;
using CampLib.Repositorya;
using KlasseLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/auth")]
public class AdminController : ControllerBase
{
    private readonly PasswordHasher<Admin> _hasher = new();
    private readonly StaffRepository _staffRepository;

    public AdminController(StaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string? Password { get; set; }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (dto == null)
            return BadRequest("DTO er null");

        var username = dto.Username?.Trim().ToLower();
        var password = dto.Password?.Trim();

        // =========================
        // 🔐 ADMIN LOGIN
        // =========================
        if (username == AdminLogin.AdminUser.Username.ToLower())
        {
            var result = _hasher.VerifyHashedPassword(
                null,
                AdminLogin.AdminUser.PasswordHash,
                password ?? ""
            );

            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Forkert admin password");

            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("Username", username);

            return Ok(new { role = "admin" });
        }

        // =========================
// 🔐 STAFF LOGIN
// =========================
        var staff = await _staffRepository.GetByUsernameAsync(username);
        if (staff != null)
        {
            if (staff.Password != password)
                return Unauthorized("Forkert staff password");

            HttpContext.Session.SetString("IsAdmin", "true"); // staff har admin-rettigheder
            HttpContext.Session.SetString("IsStaff", "true");
            HttpContext.Session.SetString("Username", username);

            return Ok(new { role = "staff" }); // 👈 VIGTIG ÆNDRING
        }

        // =========================
        // 🎓 STUDENT LOGIN
        // =========================
        if (!username.EndsWith("@edu.zealand.dk"))
            return Unauthorized("Ugyldig bruger");

        HttpContext.Session.SetString("IsStudent", "true");
        HttpContext.Session.SetString("Username", username);

        return Ok(new { role = "student" });
    }
}