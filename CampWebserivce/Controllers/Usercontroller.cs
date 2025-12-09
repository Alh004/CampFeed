using CampLib.Model;
using CampLib.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CampApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;

        public UserController()
        {
            _repository = new UserRepository();
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            try
            {
                var addedUser = await _repository.AddAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = addedUser.Id }, addedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // fx hvis email ikke matcher @edu.zealand.dk
            }
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            try
            {
                var updatedUser = await _repository.UpdateAsync(id, user);
                if (updatedUser == null) return NotFound();
                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}