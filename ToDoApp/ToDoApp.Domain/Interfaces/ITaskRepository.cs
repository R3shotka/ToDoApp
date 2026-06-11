using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Interfaces;

public interface ITaskRepository : IRepository<TaskItem>
{
    Task<IEnumerable<TaskItem>> GetByUserIdAsync(int userId);
    
}