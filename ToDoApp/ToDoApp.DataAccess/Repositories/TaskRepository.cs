using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;
using ToDoApp.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _dbContext.Tasks.ToListAsync();
    }

    public async Task<TaskItem> AddAsync(TaskItem entity)
    {
        await _dbContext.Tasks.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TaskItem entity)
    {
        _dbContext.Tasks.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem entity)
    {
        _dbContext.Tasks.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(int userId)
    {
        return await _dbContext.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int userId,
        int page,
        int pageSize,
        string? search = null,
        int? categoryId = null)
    {
        // 1. Початковий запит — всі задачі користувача
        var query = _dbContext.Tasks
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        // 2. Застосовуємо пошук (якщо є)
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(t => t.Title.Contains(search));
        }

        // 3. Застосовуємо фільтр по категорії (якщо є)
        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        // 4. Підраховуємо загальну кількість (для TotalPages)
        var totalCount = await query.CountAsync();

        // 5. Застосовуємо сортування та пагінацію
        var items = await query
            .OrderByDescending(t => t.CreatedAt)  // новіші спочатку
            .Skip((page - 1) * pageSize)          // пропустити попередні сторінки
            .Take(pageSize)                       // взяти тільки поточну сторінку
            .ToListAsync();

        // 6. Повертаємо Tuple
        return (items, totalCount);
    }
}