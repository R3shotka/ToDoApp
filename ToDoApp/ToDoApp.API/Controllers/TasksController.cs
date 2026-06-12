using Microsoft.AspNetCore.Mvc;
using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;

namespace ToDoApp.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        int userId = 1;
        
        var tasks = await _taskService.GetAllByUserAsync(userId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetById([FromRoute] int id)
    {
        int userId = 1;
        
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
        int userId = 1;

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
        int userId = 1;

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
        int userId = 1;

        var result = await _taskService.DeleteAsync(id, userId);

        if (!result)
            return NotFound();

        return NoContent();
    }
}