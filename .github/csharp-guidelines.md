# C# Backend Guidelines

## Naming Conventions

- Classes, methods, properties: **PascalCase** (e.g., `UserRepository`, `GetById`)
- Variables, parameters: **camelCase** (e.g., `userId`, `totalPrice`)
- Private fields: `_camelCase` with underscore (e.g., `_userList`)
- Constants: **UPPER_SNAKE_CASE** (e.g., `MAX_RETRY_COUNT`)

## Code Structure

- One class per file
- File name matches class name
- Use namespaces matching folder structure
- Keep methods under 50 lines

## DTO and Models

- Separate DTOs for Create/Update/Response
- Use DataAnnotations for validation: `[Required]`, `[EmailAddress]`, `[StringLength]`
- Never return domain models directly - always use DTOs

## Repository Pattern

- All data access through repositories
- Repository methods return domain models
- Register repositories as singletons
- Use thread-safe operations (lock, Interlocked)

## API Endpoints

- Return proper status codes: 200 (OK), 201 (Created), 400 (Bad Request), 404 (Not Found)
- Use `ValidationHelper.Validate()` for all input
- Always validate before repository operations
- Use minimal API style in Program.cs

## Comments

- XML comments (`///`) for public methods only
- Inline comments only when logic is non-obvious
- Avoid stating the obvious - code should be self-documenting
