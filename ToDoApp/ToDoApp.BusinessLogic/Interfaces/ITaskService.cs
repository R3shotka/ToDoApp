using ToDoApp.BusinessLogic.DTOs;

namespace ToDoApp.BusinessLogic.Interfaces;

public interface ITaskService
{
    Task<TaskDto?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<TaskDto>> GetAllByUserAsync(int userId);
    Task<TaskDto> CreateAsync(CreateTaskDto dto, int userId);
    Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
    Task<PagedResult<TaskDto>> GetPagedAsync(
        int userId,
        int page = 1,
        int pageSize = 10,
        string? search = null,
        int? categoryId = null);
}