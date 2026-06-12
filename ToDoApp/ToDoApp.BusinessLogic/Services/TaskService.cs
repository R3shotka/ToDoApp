using ToDoApp.BusinessLogic.DTOs;
using ToDoApp.BusinessLogic.Interfaces;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.BusinessLogic.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
    }


    public async Task<TaskDto?> GetByIdAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            return null;
        }
        
        return MapToDto(task);
    }

    public async Task<IEnumerable<TaskDto>> GetAllByUserAsync(int userId)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(userId);
        var dtoTasks = tasks.Select(MapToDto);
        return dtoTasks;
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto, int userId)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException("Title cannot be empty");
        }

        if (dto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
            if (category == null || category.UserId != userId)
            {
                throw new ArgumentException("Invalid category");
            }
        }

        var taskItem = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            CategoryId = dto.CategoryId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsCompleted = false
        };
        await _taskRepository.AddAsync(taskItem);
        return MapToDto(taskItem);
    }

    public async Task<TaskDto?> UpdateAsync(int id, UpdateTaskDto dto, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            return null;
        }
        
        if (dto.Title != null)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title cannot be empty");

            task.Title = dto.Title;
        }

        if (dto.Description != null)
        {
            task.Description = dto.Description;
        }

        if (dto.DueDate != null)
        {
            task.DueDate = dto.DueDate;
        }
        if(dto.IsCompleted != null)
        {
            task.IsCompleted = dto.IsCompleted.Value;
        }
        if(dto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value);
            if (category == null || category.UserId != userId)
            {
                throw new ArgumentException("Invalid category");
            }
            task.CategoryId = dto.CategoryId;
        }
        
        task.UpdatedAt = DateTime.UtcNow;
        await _taskRepository.UpdateAsync(task);
        return MapToDto(task);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.UserId != userId)
        {
            return false;
        }
        
        await _taskRepository.DeleteAsync(task);
        return true;
    }

    private TaskDto MapToDto(TaskItem taskItem)
    {
        var taskDto = new TaskDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsCompleted = taskItem.IsCompleted,
            DueDate = taskItem.DueDate,
            CategoryId = taskItem.CategoryId,
            CategoryName = taskItem.Category?.Name,
            UserId = taskItem.UserId,
            CreatedAt = taskItem.CreatedAt,
            UpdatedAt = taskItem.UpdatedAt
        };
        return taskDto;
    }
    
    public async Task<PagedResult<TaskDto>> GetPagedAsync(
        int userId,
        int page = 1,
        int pageSize = 10,
        string? search = null,
        int? categoryId = null)
    {
        // 1. Валідація параметрів
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;  // максимум 100 елементів на сторінку

        // 2. Викликаємо Repository
        var (tasks, totalCount) = await _taskRepository.GetPagedAsync(
            userId,
            page,
            pageSize,
            search,
            categoryId);

        // 3. Конвертуємо Entity → DTO
        var taskDtos = tasks.Select(MapToDto).ToList();

        // 4. Обчислюємо TotalPages
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // 5. Формуємо результат
        return new PagedResult<TaskDto>
        {
            Items = taskDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}

