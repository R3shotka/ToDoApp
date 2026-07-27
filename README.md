# ToDoApp

A full-stack task management application with JWT authentication, category-based organisation, and paginated task lists.

**Features:** user registration & login, task CRUD with search and category filtering, category management, server-side pagination, optimistic UI updates, overdue task highlighting.

---

## Tech Stack

| Layer | Technologies |
|---|---|
| Backend | .NET 10, ASP.NET Core Web API, Entity Framework Core, SQL Server, JWT Bearer (HS256) |
| Frontend | Vue 3, TypeScript, Vite, Pinia, Vue Router 4, Axios |

---

## Repository Structure

```
toDoApp/
├── ToDoApp/                          # .NET backend (Clean Architecture)
│   ├── ToDoApp.API/                  # Controllers, Program.cs, appsettings, launchSettings
│   ├── ToDoApp.BusinessLogic/        # Service layer, business rules
│   ├── ToDoApp.DataAccess/           # EF Core DbContext, migrations, repositories
│   └── ToDoApp.Domain/               # Entities, DTOs
└── todo-frontend/                    # Vue 3 frontend
    └── src/
        ├── api/                      # client.ts (axios + interceptors), tasks.ts, categories.ts, users.ts
        ├── composables/              # useTasks.ts (pagination, CRUD, filters), useCategories.ts
        ├── stores/                   # auth.ts — Pinia store (token, user, login/logout)
        ├── types/                    # index.ts — all TypeScript interfaces (DTOs)
        ├── router/                   # index.ts — Vue Router + auth navigation guards
        ├── views/                    # LoginView, RegisterView, TasksView, CategoriesView
        └── components/
            ├── tasks/                # TaskList, TaskItem, TaskForm (modal), TaskFilter
            ├── categories/           # CategoryList, CategoryItem, CategoryForm
            └── shared/               # AppHeader, AppLayout, BasePagination
```

---

## Prerequisites

- **Node.js** `^22.18.0` or `>=24.12.0`
- **.NET SDK** `10.0`
- **SQL Server** — any accessible instance works: locally installed SQL Server / SQL Server Express, a Docker container, Azure SQL, etc. The only requirement is that the connection string in `appsettings.json` points to a running instance with sufficient permissions to create the database.

---

## Getting Started

### 1. Provision SQL Server

Use whichever approach fits your setup. A few common options:

- **Locally installed SQL Server / SQL Server Express** — start the service and note your instance name.
- **Docker:**
  ```bash
  docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" \
    -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
  ```
- Any other accessible SQL Server instance — remote server, Azure SQL, etc.

### 2. Configure the Connection String

Open `ToDoApp/ToDoApp.API/appsettings.json` and update `DefaultConnection` to match your instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=ToDoAppDb;User Id=sa;Password=YOUR_PASSWORD_HERE;TrustServerCertificate=True;"
}
```

### 3. Apply EF Core Migrations

```bash
cd ToDoApp/ToDoApp.API
dotnet ef database update
```

This creates the `ToDoAppDb` database and applies all migrations.

### 4. Start the Backend

```bash
cd ToDoApp/ToDoApp.API
dotnet run
```

The API will be available at:
- **HTTPS:** `https://localhost:7083`
- **HTTP:** `http://localhost:5182`

Swagger UI (dev only): `https://localhost:7083/swagger`

### 5. Configure CORS (if needed)

The backend already allows `http://localhost:5173` (Vite default). If your frontend runs on a different port, add it to the `WithOrigins` call in `ToDoApp/ToDoApp.API/Program.cs`:

```csharp
policy.WithOrigins("http://localhost:5173", "http://your-port-here")
```

### 6. Start the Frontend

```bash
cd todo-frontend
npm install
npm run dev
```

The app will be available at `http://localhost:5173`.

---

## Core Functionality

### Authentication
- Register with username, email, and password
- Login returns a JWT token (60-minute expiry) stored in `localStorage`
- All protected routes require a valid token; expired or missing tokens redirect to `/login`

### Categories
- Full CRUD: create, rename, and delete categories
- Categories are user-scoped — each user manages their own

### Tasks
- Full CRUD with an inline modal form
- Fields: title (required), description, due date, category
- **Search** — live filtering by title with 400 ms debounce
- **Category filter** — filter tasks by a specific category
- **Pagination** — server-side, 10 tasks per page
- **Overdue highlighting** — tasks past their due date (and not yet completed) are visually flagged with a danger-coloured left border and date text
- **Optimistic toggle** — marking a task complete/incomplete updates the UI instantly and rolls back on error

---

## Key Technical Decisions

**Pinia only for auth; composables for everything else.**
The auth store (`stores/auth.ts`) needs to be globally accessible across the router (navigation guards), the Axios interceptor (to attach the token), and the header component simultaneously. That cross-cutting requirement is exactly what a global store is for. Tasks and categories, by contrast, are only needed within their own views — a plain composable (`useTasks`, `useCategories`) is simpler and sufficient.

**Optimistic toggle with explicit rollback.**
When a user checks or unchecks a task, `useTasks.toggleTask` flips `task.isCompleted` in the local array before the API call is made, so the UI responds instantly. If the `PUT /api/tasks/{id}` request fails, the original value is restored and `fetchTasks` re-syncs from the server. This avoids a full re-fetch on the happy path while keeping the list consistent on error.

**Debounce via a plain `setTimeout`, not a library.**
`TaskFilter.vue` keeps a module-level `debounceTimer` reference and uses `clearTimeout` + `setTimeout` (400 ms) inside a `watch` on the local search input. The 400 ms delay is long enough to skip most mid-word keystrokes without feeling laggy. No utility library is pulled in for a single use.

**Overdue detection is day-level, not datetime-level.**
`TaskItem.vue` zeroes out the time component of both `new Date(task.dueDate)` and `new Date()` before comparing, so a task due today is never shown as overdue regardless of the exact time it was created or when the page loads. Completed tasks are excluded unconditionally — the `isCompleted` check happens before the date comparison.

**Categories fetched once in the view, passed down as props.**
`TasksView` calls `useCategories().fetchAll()` on mount and passes the resulting array as a prop to both `TaskFilter` and `TaskForm`. This means a single API request serves both the filter dropdown and the task form's category selector, with no duplicated fetch logic or shared reactive state between sibling components.

---

## API Reference

All task and category endpoints require `Authorization: Bearer <token>`.

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/api/users/register` | — | Register a new user |
| `POST` | `/api/users/login` | — | Login, returns JWT token |
| `GET` | `/api/users/{id}` | — | Get user by ID |
| `GET` | `/api/tasks` | ✅ | List tasks (paginated, filterable) |
| `GET` | `/api/tasks/{id}` | ✅ | Get task by ID |
| `POST` | `/api/tasks` | ✅ | Create task |
| `PUT` | `/api/tasks/{id}` | ✅ | Update task |
| `DELETE` | `/api/tasks/{id}` | ✅ | Delete task |
| `GET` | `/api/categories` | ✅ | List all categories |
| `GET` | `/api/categories/{id}` | ✅ | Get category by ID |
| `POST` | `/api/categories` | ✅ | Create category |
| `PUT` | `/api/categories/{id}` | ✅ | Update category |
| `DELETE` | `/api/categories/{id}` | ✅ | Delete category |

Full interactive docs: `https://localhost:7083/swagger` (available when backend is running in Development mode).
