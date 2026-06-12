# 📝 ToDoApp - Task Management Application

> Сучасний REST API для управління задачами з JWT авторизацією, пошуком та категоризацією

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)

---

## 🎯 Про проєкт

ToDoApp — це повнофункціональний backend для управління задачами, розроблений згідно з принципами Clean Architecture. Проєкт створено як демонстрація навичок роботи з .NET, Entity Framework Core, JWT авторизацією та сучасними патернами проєктування.

### ✨ Ключові можливості

- 🔐 **JWT авторизація** — безпечна автентифікація з Bearer токенами
- 📄 **Пагінація** — ефективна робота з великими списками (max 100 елементів/сторінку)
- 🔍 **Пошук** — миттєвий пошук задач по назві
- 🏷️ **Категорії** — організація задач за категоріями
- 👤 **Мультикористувацька система** — кожен користувач бачить тільки свої дані
- 🔒 **BCrypt хешування** — безпечне зберігання паролів

---

## 🛠 Технологічний стек

### Backend
- **Framework:** .NET 10.0 (ASP.NET Core Web API)
- **ORM:** Entity Framework Core 10.0
- **Database:** MS SQL Server 2022
- **Authentication:** JWT (JSON Web Tokens)
- **Password Hashing:** BCrypt.Net-Next
- **API Documentation:** Swagger/OpenAPI

### Архітектура
```
┌─────────────────────────────────────────┐
│  Controllers (API Layer)                │  ← HTTP Requests/Responses
├─────────────────────────────────────────┤
│  Services (Business Logic)              │  ← Валідація, правила
├─────────────────────────────────────────┤
│  Repositories (Data Access)             │  ← EF Core запити
├─────────────────────────────────────────┤
│  Domain (Entities + Interfaces)         │  ← Моделі БД
└─────────────────────────────────────────┘
```

**Патерни:**
- ✅ Repository Pattern
- ✅ Dependency Injection
- ✅ DTO (Data Transfer Objects)
- ✅ 4-tier Architecture

---

## 📚 API Endpoints

### 🔑 Authentication

| Method | Endpoint | Опис | Auth |
|--------|----------|------|------|
| `POST` | `/api/users/register` | Реєстрація нового користувача | ❌ |
| `POST` | `/api/users/login` | Логін (повертає JWT токен) | ❌ |

**Приклад реєстрації:**
```json
POST /api/users/register
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123!"
}
```

**Приклад логіну:**
```json
POST /api/users/login
{
  "username": "john_doe",
  "password": "SecurePass123!"
}

// Відповідь:
{
  "id": 1,
  "username": "john_doe",
  "email": "john@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

### ✅ Tasks

| Method | Endpoint | Опис | Auth |
|--------|----------|------|------|
| `GET` | `/api/tasks` | Список задач (з параметрами) | ✅ |
| `GET` | `/api/tasks/{id}` | Одна задача | ✅ |
| `POST` | `/api/tasks` | Створити задачу | ✅ |
| `PUT` | `/api/tasks/{id}` | Оновити задачу | ✅ |
| `DELETE` | `/api/tasks/{id}` | Видалити задачу | ✅ |

**Query параметри для GET /api/tasks:**
- `page` — номер сторінки (default: `1`)
- `pageSize` — кількість елементів (default: `10`, max: `100`)
- `search` — пошук по назві (Title)
- `categoryId` — фільтр по категорії

**Приклади запитів:**
```bash
# Перша сторінка, 10 задач
GET /api/tasks?page=1&pageSize=10

# Пошук задач з "buy"
GET /api/tasks?search=buy

# Тільки задачі категорії Work (id=1)
GET /api/tasks?categoryId=1

# Комбінація всіх параметрів
GET /api/tasks?page=2&pageSize=15&search=milk&categoryId=2
```

**Приклад створення задачі:**
```json
POST /api/tasks
Authorization: Bearer {token}

{
  "title": "Buy groceries",
  "description": "Milk, bread, eggs",
  "categoryId": 2,
  "dueDate": "2026-06-15T10:00:00Z"
}
```

**Відповідь з пагінацією:**
```json
{
  "items": [
    {
      "id": 1,
      "title": "Buy milk",
      "description": "2 liters",
      "isCompleted": false,
      "categoryId": 2,
      "categoryName": "Personal",
      "userId": 1,
      "createdAt": "2026-06-12T10:00:00Z"
    }
  ],
  "totalCount": 47,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

---

### 🏷️ Categories

| Method | Endpoint | Опис | Auth |
|--------|----------|------|------|
| `GET` | `/api/categories` | Список категорій | ✅ |
| `GET` | `/api/categories/{id}` | Одна категорія | ✅ |
| `POST` | `/api/categories` | Створити категорію | ✅ |
| `PUT` | `/api/categories/{id}` | Оновити категорію | ✅ |
| `DELETE` | `/api/categories/{id}` | Видалити категорію | ✅ |

**Приклад:**
```json
POST /api/categories
Authorization: Bearer {token}

{
  "name": "Work"
}
```

---

## 🚀 Швидкий старт

### Передумови

Перед початком переконайся що встановлено:
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

---

### Крок 1️⃣: Клонувати репозиторій

```bash
git clone https://github.com/R3shotka/ToDoApp.git
cd ToDoApp
```

---

### Крок 2️⃣: Запустити SQL Server

**Якщо контейнер вже існує:**
```bash
docker start sql_server
```

**Якщо контейнер не створений:**
```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrongPass123!" \
  -p 1433:1433 \
  --name sql_server \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

---

### Крок 3️⃣: Налаштувати Connection String

Створи файл `ToDoApp.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ToDoAppDb;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;"
  }
}
```

⚠️ **Важливо:** Використай той самий пароль, що вказав в Docker команді!

---

### Крок 4️⃣: Застосувати міграції бази даних

```bash
cd ToDoApp
dotnet ef database update --project ToDoApp.DataAccess --startup-project ToDoApp.API
```

✅ Це створить базу даних `ToDoAppDb` та всі таблиці.

---

### Крок 5️⃣: Запустити API

```bash
cd ToDoApp.API
dotnet run
```

🎉 **API запущено!** Відкрий браузер:
- Swagger UI: `https://localhost:5001/swagger`
- API Base: `https://localhost:5001/api`

**Для auto-reload при змінах:**
```bash
dotnet watch run
```

---

## 🔐 Робота з авторизацією в Swagger

### Крок 1: Створи акаунт
1. Відкрий Swagger UI: `https://localhost:5001/swagger`
2. Викличи `POST /api/users/register`:
   ```json
   {
     "username": "testuser",
     "email": "test@example.com",
     "password": "MyPassword123"
   }
   ```

### Крок 2: Отримай токен
3. Викличи `POST /api/users/login` з тими самими credentials
4. **Скопіюй `token`** з відповіді (довгий рядок, починається з `eyJ...`)

### Крок 3: Авторизуйся
5. Натисни кнопку **🔒 Authorize** у правому верхньому куті Swagger
6. Вставмо **тільки токен** (без слова "Bearer")
7. Натисни **Authorize** → **Close**

✅ Тепер іконка замка 🔒 закрита — можеш використовувати всі endpoints!

---

## 📂 Структура проєкту

```
ToDoApp/
│
├── ToDoApp.API/                      # 🌐 REST API Layer
│   ├── Controllers/                  # HTTP endpoints
│   │   ├── TasksController.cs
│   │   ├── CategoriesController.cs
│   │   └── UsersController.cs
│   ├── Program.cs                    # Конфігурація (DI, JWT, Swagger)
│   └── appsettings.json
│
├── ToDoApp.BusinessLogic/            # 💼 Business Logic Layer
│   ├── Services/                     # Бізнес-логіка + валідація
│   │   ├── TaskService.cs
│   │   ├── CategoryService.cs
│   │   ├── UserService.cs
│   │   └── JwtService.cs
│   ├── DTOs/                         # Data Transfer Objects
│   │   ├── TaskDto.cs
│   │   ├── CategoryDto.cs
│   │   ├── UserDto.cs
│   │   └── PagedResult.cs
│   └── Interfaces/                   # Service інтерфейси
│
├── ToDoApp.DataAccess/               # 💾 Data Access Layer
│   ├── Context/
│   │   └── ApplicationDbContext.cs   # EF Core DbContext
│   ├── Repositories/                 # Repository Pattern
│   │   ├── TaskRepository.cs
│   │   ├── CategoryRepository.cs
│   │   └── UserRepository.cs
│   └── Migrations/                   # EF Core міграції
│
└── ToDoApp.Domain/                   # 📦 Domain Layer
    ├── Entities/                     # Моделі БД
    │   ├── TaskItem.cs
    │   ├── Category.cs
    │   └── User.cs
    └── Interfaces/                   # Repository інтерфейси
        ├── IRepository.cs
        ├── ITaskRepository.cs
        ├── ICategoryRepository.cs
        └── IUserRepository.cs
```

---

## 🧪 Тестування

### Через Swagger UI (рекомендовано)
1. Запусти проєкт: `dotnet run`
2. Відкрий: `https://localhost:5001/swagger`
3. Виконай запити через UI

### Через curl

**Register:**
```bash
curl -X POST https://localhost:5001/api/users/register \
  -H "Content-Type: application/json" \
  -d '{"username":"test","email":"test@mail.com","password":"Pass123"}' \
  -k
```

**Login:**
```bash
curl -X POST https://localhost:5001/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"Pass123"}' \
  -k
```

**Get Tasks (з токеном):**
```bash
curl -X GET https://localhost:5001/api/tasks \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -k
```

---

## 🔧 Корисні команди

### EF Core Migrations

```bash
# Створити нову міграцію
dotnet ef migrations add MigrationName --project ToDoApp.DataAccess --startup-project ToDoApp.API

# Застосувати міграції
dotnet ef database update --project ToDoApp.DataAccess --startup-project ToDoApp.API

# Видалити останню міграцію
dotnet ef migrations remove --project ToDoApp.DataAccess --startup-project ToDoApp.API

# Видалити базу даних
dotnet ef database drop --project ToDoApp.DataAccess --startup-project ToDoApp.API --force
```

### Build & Run

```bash
# Очистити артефакти збірки
dotnet clean

# Зібрати проєкт
dotnet build

# Запустити
dotnet run --project ToDoApp.API

# Запустити з auto-reload
dotnet watch run --project ToDoApp.API
```

---

## 📝 Технічні рішення

### 🔐 Безпека
- **JWT токени** з 60-хвилинним lifetime
- **BCrypt** хешування паролів (cost factor 11)
- **`[Authorize]`** атрибути на всіх захищених endpoints
- Connection strings в `appsettings.Development.json` (не комітяться в Git)

### 🎯 Performance
- **Пагінація** з обмеженням max 100 елементів
- **`.Include()`** для завантаження пов'язаних даних (уникає N+1 проблеми)
- **Асинхронні методи** (`async/await`) для всіх операцій БД
- **Індекси** на Username та Email для швидкого пошуку

### 🏗 Архітектура
- **Dependency Injection** для всіх сервісів та репозиторіїв
- **Repository Pattern** відокремлює бізнес-логіку від data access
- **DTO Pattern** контролює які дані передаються через API
- **Navigation Properties** в EF Core для зв'язків між таблицями

---

## 📄 Ліцензія

Цей проєкт створено в освітніх цілях.

---

## 👨‍💻 Автор

**Matvii**  
📧 Email: [your-email@example.com](mailto:your-email@example.com)  
🔗 GitHub: [@R3shotka](https://github.com/R3shotka)

---

## 🙏 Подяки

Створено як тестове завдання для **SoftPlus Ukraine**.

---

<div align="center">

**⭐ Поставте зірку, якщо проєкт вам сподобався!**

Made with ❤️ and .NET

</div>
