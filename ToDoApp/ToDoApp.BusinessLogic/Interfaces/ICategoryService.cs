using ToDoApp.BusinessLogic.DTOs;

namespace ToDoApp.BusinessLogic.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetByIdAsync(int id, int userId);
    Task<IEnumerable<CategoryDto>> GetAllByUserAsync(int userId);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, int userId);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}