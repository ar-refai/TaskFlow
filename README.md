# TaskFlow API

A **production-grade REST API** for task management built with **.NET 8**, following **Onion Architecture** and **Domain-Driven Design (DDD)** principles.

## 🎯 Project Purpose

This project demonstrates best practices for building maintainable, testable, and scalable enterprise applications using:

- **Onion Architecture** (strict dependency inversion)
- **Domain-Driven Design** (rich domain models, value objects, aggregates)
- **CQRS Pattern** (manual implementation without MediatR)
- **Result Pattern** (explicit error handling)
- **Repository Pattern** with Unit of Work
- **EF Core** as a state manager (not data access abstraction)
- **RESTful API** design with proper HTTP semantics

## 🏗️ Architecture

### Layer Structure
```
TaskFlow/
├── TaskFlow.Domain          # Core business logic (zero dependencies)
├── TaskFlow.Application     # Use cases, handlers (depends on Domain only)
├── TaskFlow.Infrastructure  # EF Core, repositories (depends on Domain only)
└── TaskFlow.API             # HTTP layer, controllers (depends on all layers)
```

### Dependency Flow
```
API → Application → Domain
  ↘    Infrastructure ↗
```

**Key Rule:** Dependencies point **inward**. Inner layers never reference outer layers.

---

## 🚀 Features

### Domain Model
- **Entities:** Task, Project, TeamMember, Comment
- **Value Objects:** TaskId, ProjectId, TeamMemberId, CommentId, Priority, TaskStatus, DateRange, Tag
- **Aggregates:** Task (root), Comment (child)
- **Domain Events:** TaskAssigned, TaskCompleted, TaskStatusChanged
- **Business Rules:** State machine for task status transitions, invariant validation

### API Endpoints (20 Total)

#### Projects (5 endpoints)
- `POST /api/projects` — Create project
- `GET /api/projects` — List all projects
- `GET /api/projects/{id}` — Get project by ID
- `PUT /api/projects/{id}` — Update project
- `DELETE /api/projects/{id}` — Delete project

#### Tasks (10 endpoints)
- `POST /api/projects/{projectId}/tasks` — Create task in project
- `GET /api/projects/{projectId}/tasks` — Get all tasks in project
- `GET /api/tasks?status=...&assigneeId=...` — Filter tasks (query parameters)
- `GET /api/tasks/{id}` — Get task by ID
- `PUT /api/tasks/{id}` — Update task details
- `DELETE /api/tasks/{id}` — Delete task
- `PUT /api/tasks/{id}/assign` — Assign task to team member
- `PUT /api/tasks/{id}/status` — Change task status
- `POST /api/tasks/{id}/comments` — Add comment to task
- `PUT /api/tasks/{id}/tags` — Update task tags

#### Team Members (5 endpoints)
- `POST /api/team-members` — Create team member
- `GET /api/team-members` — List all team members
- `GET /api/team-members/{id}` — Get team member by ID
- `PUT /api/team-members/{id}` — Update team member
- `DELETE /api/team-members/{id}` — Delete team member

---

## 🛠️ Technologies

| Layer | Technologies |
|---|---|
| **API** | ASP.NET Core 8, Swagger (OpenAPI) |
| **Application** | CQRS (manual), Result Pattern |
| **Infrastructure** | EF Core 8, SQL Server, Fluent API |
| **Domain** | C# 12, Record types, Value objects |
| **DevOps** | Docker (SQL Server), Git |

---

## 📦 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository:**
```bash
   git clone https://github.com/YOUR_USERNAME/TaskFlow.git
   cd TaskFlow
```

2. **Start SQL Server (Docker):**
```bash
   docker-compose up -d
```

3. **Restore dependencies:**
```bash
   dotnet restore
```

4. **Run migrations:**
```bash
   cd src/TaskFlow.Infrastructure
   dotnet ef database update --startup-project ../TaskFlow.API
   cd ../..
```

5. **Run the API:**
```bash
   dotnet run --project src/TaskFlow.API
```

6. **Open Swagger UI:**
```
   http://localhost:5000 (or the port shown in terminal)
```

---

## 🧪 Testing

End-to-end testing via Swagger UI:

1. Create a project
2. Create a team member
3. Create a task in the project
4. Assign the task to the team member
5. Change task status through valid transitions
6. Add comments and tags
7. Test error cases (invalid status transitions, duplicate emails, etc.)

---

## 📚 Learning Highlights

### Domain-Driven Design Patterns
- ✅ **Entities** with identity and lifecycle
- ✅ **Value Objects** (immutable, equality by value)
- ✅ **Aggregates** (TaskEntity owns Comments)
- ✅ **Domain Events** (TaskAssigned, TaskCompleted)
- ✅ **Domain Exceptions** (InvalidStatusTransitionException)

### Onion Architecture Principles
- ✅ **Dependency Inversion** (interfaces in Domain, implementations in Infrastructure)
- ✅ **Separation of Concerns** (each layer has single responsibility)
- ✅ **Testability** (domain logic has zero dependencies)

### CQRS Implementation
- ✅ **Commands** (state changes, return Result)
- ✅ **Queries** (read-only, return Result<T>)
- ✅ **Handlers** (one per use case, explicit dependencies)

### EF Core Best Practices
- ✅ **Fluent API** (zero data annotations in domain)
- ✅ **Value Conversions** (value objects ↔ primitives)
- ✅ **Owned Entities** (DateRange, Tags)
- ✅ **Backing Fields** (encapsulation, read-only properties)
- ✅ **Interceptors** (automatic CreatedAt/UpdatedAt)

---

## 📊 Project Statistics

| Metric | Count |
|---|---|
| **Total Endpoints** | 20 |
| **Domain Entities** | 4 (Task, Project, TeamMember, Comment) |
| **Value Objects** | 9 (IDs, enums, DateRange, Tag) |
| **Handlers** | 20 (13 commands, 7 queries) |
| **Database Tables** | 5 (Projects, Tasks, TeamMembers, Comments, TaskTags) |
| **Lines of Code** | ~3,500 |

---

## 🎓 Key Concepts Demonstrated

1. **Strict Layering** — No shortcuts, proper dependency management
2. **Rich Domain Models** — Business logic lives in entities, not services
3. **Value Objects** — Type safety, immutability, validation
4. **Result Pattern** — Explicit success/failure without exceptions
5. **CQRS** — Separate read and write concerns
6. **Repository Pattern** — Data access abstraction
7. **Unit of Work** — Transaction coordination
8. **Global Exception Handling** — Centralized error mapping
9. **Fluent API** — Clean domain layer, configuration in Infrastructure
10. **Aggregate Boundaries** — Comment can only be created through Task

---

## 🚧 Roadmap / Future Enhancements

- [ ] JWT Authentication & Authorization
- [ ] Unit Testing (xUnit + FluentAssertions)
- [ ] Integration Testing
- [ ] Soft Delete (IsDeleted flag)
- [ ] Domain Event Dispatching
- [ ] Pagination & Sorting
- [ ] FluentValidation
- [ ] Logging (Serilog)
- [ ] API Versioning
- [ ] Rate Limiting

---

## 📝 License

This project is for educational purposes. Feel free to use it as a reference or template for your own projects.

---

## 🙏 Acknowledgments

Built as a hands-on learning project to master:
- Onion Architecture
- Domain-Driven Design
- CQRS Pattern
- EF Core as a state manager
- REST API best practices
