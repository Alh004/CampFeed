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
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Username))
            return BadRequest("Username må ikke være tom");

        string username = dto.Username.Trim();
        string? password = dto.Password;

   
        if (username.Equals(AdminLogin.AdminUser.Username, StringComparison.OrdinalIgnoreCase))
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

            return Ok(new { role = "admin", username });
        }

      
        if (!username.EndsWith("@edu.zealand.dk", StringComparison.OrdinalIgnoreCase))
            return Unauthorized("Ugyldig username");

        HttpContext.Session.SetString("IsStudent", "true");
        HttpContext.Session.SetString("Username", username);

        return Ok(new { role = "student", username });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok("Logget ud");
    }
}