using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;
using ToDoApp.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<Category?> GetByIdAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        return categories;
    }

    public async Task<Category> AddAsync(Category entity)
    {
        await _dbContext.Categories.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Category entity)
    {
        _dbContext.Categories.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category entity)
    {
        _dbContext.Categories.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetByUserIdAsync(int userId)
    {
        var categories = await _dbContext.Categories.Where(c => c.UserId == userId).ToListAsync();
        return categories;
    }
}