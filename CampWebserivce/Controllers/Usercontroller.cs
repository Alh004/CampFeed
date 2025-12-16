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
            if (user == null) 
                return NotFound();

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            var added = await _repository.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = added.Iduser }, added);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            var updated = await _repository.UpdateAsync(id, user);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
    }
}