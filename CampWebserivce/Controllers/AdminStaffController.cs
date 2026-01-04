using Microsoft.AspNetCore.Mvc;
using CampLib.Repository;
using CampLib.Model;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/admin/staff")]
public class AdminStaffController : ControllerBase
{
    private readonly StaffRepository _staffRepository;

    public AdminStaffController(StaffRepository staffRepository)
    {
        _staffRepository = staffRepository;
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("IsAdmin") == "true";
    }

    // =========================
    // GET ALL STAFF
    // =========================
    [HttpGet]
    public async Task<IActionResult> GetAllStaff()
    {
        if (!IsAdmin())
            return Unauthorized("Kun admin har adgang");

        var staffList = await _staffRepository.GetAllAsync();
        return Ok(staffList);
    }

    // =========================
    // CREATE STAFF
    // =========================
    [HttpPost]
    public async Task<IActionResult> CreateStaff([FromBody] Staff staffInput)
    {
        if (!IsAdmin())
            return Unauthorized("Kun admin har adgang");

        if (!staffInput.Username.EndsWith("@edu.zealand.dk"))
            return BadRequest("Staff skal have @edu.zealand.dk");

        // Brug constructor til at lave objektet
        var staff = new Staff(0, staffInput.Username, staffInput.Password);

        var created = await _staffRepository.AddAsync(staff);

        return Ok(new
        {
            Message = "Staff oprettet",
            created.Id,
            created.Username
        });
    }

    // =========================
    // DELETE STAFF
    // =========================
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        if (!IsAdmin())
            return Unauthorized("Kun admin har adgang");

        var deleted = await _staffRepository.DeleteAsync(id);

        if (!deleted)
            return NotFound("Staff findes ikke");

        return Ok("Staff slettet");
    }
}
