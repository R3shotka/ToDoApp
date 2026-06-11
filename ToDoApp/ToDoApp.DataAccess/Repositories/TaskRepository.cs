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
        return await _dbContext.Tasks.FindAsync(id);
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
}