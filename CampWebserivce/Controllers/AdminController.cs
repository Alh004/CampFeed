using CampLib.Model;
using KlasseLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/auth")]
public class AdminController : ControllerBase
{
    private readonly PasswordHasher<Admin> _hasher = new();

    public class LoginDto
    {
        public string Username { get; set; }
        public string? Password { get; set; }
    }

    // =======================
    // LOGIN ENDPOINT
    // =======================
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (dto == null)
            return BadRequest("DTO er null");

        Console.WriteLine("=== Modtaget fra frontend ===");
        Console.WriteLine($"Username: '{dto.Username}'");
        Console.WriteLine($"Password: '{dto.Password}'");

        string username = dto.Username?.Trim() ?? "";
        string? password = dto.Password?.Trim();

        // Admin login
        if (username.Equals(AdminLogin.AdminUser.Username, StringComparison.OrdinalIgnoreCase))
        {
            var result = _hasher.VerifyHashedPassword(null, AdminLogin.AdminUser.PasswordHash, password ?? "");
            Console.WriteLine("VerifyHashedPassword result: " + result);

            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Forkert admin password");

            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("Username", username);
            return Ok(new { role = "admin", username });
        }

        // Student login
        if (!username.EndsWith("@edu.zealand.dk", StringComparison.OrdinalIgnoreCase))
            return Unauthorized("Ugyldig username");

        HttpContext.Session.SetString("IsStudent", "true");
        HttpContext.Session.SetString("Username", username);
        return Ok(new { role = "student", username });
    }

    // =======================
    // GET CURRENT USER
    // =======================
    [HttpGet("me")]
    public IActionResult Me()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
        {
            return Ok(new { role = "admin", username = HttpContext.Session.GetString("Username") });
        }

        if (HttpContext.Session.GetString("IsStudent") == "true")
        {
            return Ok(new { role = "student", username = HttpContext.Session.GetString("Username") });
        }

        return Unauthorized("Ikke logget ind");
    }
}
