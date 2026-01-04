using Microsoft.AspNetCore.Mvc;
using CampLib.Repository;
using CampLib.Model;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/staff/login")]
    public class StaffLoginController : ControllerBase
    {
        private readonly StaffRepository _staffRepository;

        public StaffLoginController(StaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Staff staffDto)
        {
            var staff = await _staffRepository.GetByUsernameAsync(staffDto.Username);

            if (staff == null || staff.Password != staffDto.Password)
                return Unauthorized("Forkert brugernavn eller password");

            return Ok(new
            {
                Message = "Login succesfuldt",
                staff.Id,
                staff.Username
            });
        }
    }
}