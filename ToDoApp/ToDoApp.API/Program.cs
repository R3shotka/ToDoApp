using Microsoft.EntityFrameworkCore;
using ToDoApp.BusinessLogic.Interfaces;
using ToDoApp.BusinessLogic.Services;
using ToDoApp.DataAccess.Context;
using ToDoApp.DataAccess.Repositories;
using ToDoApp.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Додаємо DbContext з SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Реєструємо Repositories
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 3. Реєструємо Services
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();

// 4. Додаємо Controllers
builder.Services.AddControllers();

// 5. Додаємо Swagger для тестування API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 6. Налаштування CORS (для Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Використовуємо CORS
app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();