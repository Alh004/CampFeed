using KlasseLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueController : Controller
    {
        private readonly IssueRepository _repository;

        public IssueController(IssueRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        // GET: IssueController
        public async Task<ActionResult<List<Issue>>> GetAll()
        {
            var issues = await _repository.GetAllAsync();
            return Ok(issues);
        }

        // GET api/issue/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Issue>> GetById(int id)
        {
            var issue = await _repository.GetByIdAsync(id);

            if (issue == null)
                return NotFound($"Issue with ID {id} not found.");

            return Ok(issue);
        }

        // POST api/issue
        [HttpPost]
        public async Task<ActionResult> Create(Issue issue)
        {
            if (issue == null)
                return BadRequest("Issue cannot be null.");

            int rows = await _repository.CreateAsync(issue);

            if (rows == 0)
                return StatusCode(500, "Could not create issue.");

            return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
        }

        // PUT api/issue/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, Issue update)
        {
            if (id != update.Id)
                return BadRequest("ID in URL does not match issue ID.");

            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
                return NotFound($"Issue with ID {id} not found.");

            bool updated = await _repository.UpdateAsync(update);

            if (!updated)
                return StatusCode(500, "Issue could not be updated.");

            return NoContent();
        }
    }
}
