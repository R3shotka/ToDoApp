using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetByUserIdAsync(int userId);
}