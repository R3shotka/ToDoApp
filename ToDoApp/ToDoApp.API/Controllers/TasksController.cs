using Microsoft.AspNetCore.Mvc;
using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ToDoApp.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TaskDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _taskService.GetPagedAsync(userId, page, pageSize, search, categoryId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetById([FromRoute] int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var task = await _taskService.GetByIdAsync(id,  userId);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var created = await _taskService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> Update([FromRoute] int id, [FromBody] UpdateTaskDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            var updated = await _taskService.UpdateAsync(id, dto, userId);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _taskService.DeleteAsync(id, userId);

        if (!result)
            return NotFound();

        return NoContent();
    }
}