using Microsoft.AspNetCore.Mvc;
using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;

namespace ToDoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginUserDto dto)
    {
        var user = await _userService.LoginAsync(dto);

        if (user == null)
            return Unauthorized(new { message = "Invalid username or password" });

        return Ok(user);
    }
}
