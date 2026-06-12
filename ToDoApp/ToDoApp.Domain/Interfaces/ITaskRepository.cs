using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Interfaces;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetByUserIdAsync(int userId);
    Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int userId,
        int page,
        int pageSize,
        string? search = null,
        int? categoryId = null);
}