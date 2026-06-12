using Microsoft.AspNetCore.Mvc;
using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;

namespace ToDoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        int userId = 1;

        var categories = await _categoryService.GetAllByUserAsync(userId);
        return Ok(categories);
    }

    // GET: api/categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        int userId = 1;

        var category = await _categoryService.GetByIdAsync(id, userId);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
    {
        int userId = 1;

        try
        {
            var created = await _categoryService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/categories/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        int userId = 1;

        try
        {
            var updated = await _categoryService.UpdateAsync(id, dto, userId);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/categories/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        int userId = 1;

        var result = await _categoryService.DeleteAsync(id, userId);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
