using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.BusinessLogic.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id, int userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        // Перевірка: категорія існує і належить користувачу
        if (category == null || category.UserId != userId)
            return null;

        return MapToDto(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllByUserAsync(int userId)
    {
        var categories = await _categoryRepository.GetByUserIdAsync(userId);
        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, int userId)
    {
        // Валідація
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Category name cannot be empty");

        // Створюємо Entity
        var category = new Category
        {
            Name = dto.Name,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _categoryRepository.AddAsync(category);
        return MapToDto(created);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, int userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null || category.UserId != userId)
            return null;

        // Валідація
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Category name cannot be empty");

        category.Name = dto.Name;

        await _categoryRepository.UpdateAsync(category);
        return MapToDto(category);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null || category.UserId != userId)
            return false;

        await _categoryRepository.DeleteAsync(category);
        return true;
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UserId = category.UserId,
            CreatedAt = category.CreatedAt
        };
    }
}