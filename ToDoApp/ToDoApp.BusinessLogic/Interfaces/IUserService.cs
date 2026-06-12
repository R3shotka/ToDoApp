using ToDoApp.BusinessLogic.DTOs;

namespace ToDoApp.BusinessLogic.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> RegisterAsync(RegisterUserDto dto);
    Task<LoginResponseDto?> LoginAsync(LoginUserDto dto);
}