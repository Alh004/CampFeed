using CampApi.DTO;
using CampLib.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KlasseLib;

namespace CampApi.Controllers;
a
[ApiController]
[Route("api/[controller]")]
//Added by 

public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    public AuthController(AppDbContext ctx) { _context = ctx; }

    [HttpPost("check")]
    public async Task<IActionResult> CheckUser([FromBody] EmailDto dto)
    {
        if (!dto.Email.EndsWith("@edu.zealand.dk"))
            return BadRequest("Email must be a Zealand student email.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            user = new User { Email = dto.Email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        return Ok(new { userId = user.Iduser });
    }
}

public class EmailDto
{
    public string Email { get; set; }
}
