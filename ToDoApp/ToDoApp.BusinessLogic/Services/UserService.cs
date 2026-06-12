using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public UserService(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
            return null;

        return MapToDto(user);
    }

    public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
    {
        // Валідація
        if (string.IsNullOrWhiteSpace(dto.Username))
            throw new ArgumentException("Username cannot be empty");

        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email cannot be empty");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password cannot be empty");

        // Перевірка: username унікальний
        var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
            throw new ArgumentException("Username already exists");

        // Перевірка: email унікальний
        var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
            throw new ArgumentException("Email already exists");

        // Хешування пароля (поки що просто зберігаємо, пізніше додамо BCrypt)
        var passwordHash = HashPassword(dto.Password);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.AddAsync(user);
        return MapToDto(created);
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);

        if (user == null)
            return null;

        // Перевірка пароля
        if (!VerifyPassword(dto.Password, user.PasswordHash))
            return null;

        // Генеруємо JWT токен
        var token = _jwtService.GenerateToken(user.Id, user.Username);

        return new LoginResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Token = token
        };
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}