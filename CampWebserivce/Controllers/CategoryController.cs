using CampLib.Repository;
using KlasseLib;
using Microsoft.AspNetCore.Mvc;
d
namespace StudyFeedback.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryRepository _repo;

    public CategoryController(CategoryRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _repo.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _repo.GetByIdAsync(id);
        return category == null ? NotFound() : Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _repo.AddAsync(category);
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category model)
    {
        var updated = await _repo.UpdateAsync(id, model);
        return updated == null ? NotFound() : Ok(updated);
    }
    
    }
