This repository is a small ASP.NET Core minimal API (net9.0). Below are concise, actionable instructions an AI coding agent should follow to be productive here.

Architecture & why

- Minimal API: `Program.cs` is the single wiring place (DI + route registration). Prefer small, local edits here for new endpoints.
- In-memory domain: `Models/` (DTOs/models), `Repositories/` (thread-safe in-memory stores). This keeps the project zero-dependency and easy to run.
- Validation: DataAnnotations on DTOs + explicit call to `ValidationHelper.Validate(dto)` in endpoints to return `Results.ValidationProblem(...)`.

Quick file checklist (read before editing)

- `Program.cs` — add endpoints, register repositories (singletons), and wire OpenAPI in dev.
- `Models/*.cs` — DTOs and model shapes (Users, Orders, Products).
- `Repositories/*.cs` — in-memory behavior, locking, and id generation (follow existing Interlocked pattern).
- `Utils/ValidationHelper.cs` — how endpoints expect validation to be invoked.
- `test.http` — canonical curl/http examples; update when you change routes.

Conventions & patterns

- Register repositories as singletons: `builder.Services.AddSingleton<MyRepository>()`.
- Repositories: use a private lock + System.Threading.Interlocked for id increments; return copies (ToArray) to avoid external mutation.
- DTOs: use System.ComponentModel.DataAnnotations; endpoints must call `ValidationHelper.Validate(dto)` and return immediately if non-null.
- Do uniqueness/cross-field checks at endpoint-level (example: `UserRepository.ExistsByEmail(...)`).
- Trim input strings inside repository create/update methods (`dto.Name!.Trim()`) after validation.

Build / run / debug

- Build: `dotnet build` at project root.
- Run: `dotnet run` (dev URLs from `Properties/launchSettings.json`: http://localhost:5215, https://localhost:7196).
- Quick manual tests: use `test.http` or the curl examples inside it (contains user creation and `GET /api/users/search` example).

Safety rules for automated edits

- Preserve public API routes unless user agrees to breaking changes. If breaking, update `test.http` and note the change.
- When modifying models, update DTOs, repository create/update logic, and example requests in `test.http`.
- Keep DI registrations centralized in `Program.cs`.

If you're unsure whether a change is safe (breaking API, persistence migration), ask the user before proceeding.

End of instructions.This repository is a minimal ASP.NET Core Web API (net9.0). Below are targeted, actionable instructions an AI coding agent should follow to be productive here.

Quick architecture summary

- Minimal API: `Program.cs` contains top-level routing and DI registration. Treat it as the single place that wires endpoints and services.
- Domain layers (in-memory):
  - `Models/` — DTOs and record models (e.g., `User`, `Order`, and their Create/Update DTOs).
  - `Repositories/` — in-memory, thread-safe storage classes (`UserRepository`, `OrderRepository`).
  - `Utils/ValidationHelper.cs` — DataAnnotations-based validation; endpoints call `ValidationHelper.Validate(dto)` and expect an `IResult?`.

Key files to read before editing

- `Program.cs` (routing, DI) — add/remove endpoints and register services here.
- `Models/*` (e.g., `UserModels.cs`, `OrderModels.cs`) — update shapes and DataAnnotation attributes here.
- `Repositories/*` (e.g., `UserRepository.cs`, `OrderRepository.cs`) — change persistence logic here.
- `test.http` — canonical example requests (used to sanity-check changes).
- `Properties/launchSettings.json` — dev URLs: HTTP http://localhost:5215, HTTPS https://localhost:7196

Conventions & idioms to follow

- Register in-memory repositories as singletons in `Program.cs` (e.g., `builder.Services.AddSingleton<UserRepository>()`).
- Repositories are thread-safe using a private lock and Interlocked for id generation; preserve that pattern when adding new repositories.
- DTO validation is explicit: call `ValidationHelper.Validate(dto)` in endpoints and return the result if not null. Use DataAnnotations on DTO properties.
- Perform cross-field or uniqueness checks at endpoint level (example: `UserRepository.ExistsByEmail(...)`).
- Trim string inputs in repositories when creating/updating (e.g., `dto.Name!.Trim()`). Use null-forgiving (`!`) only after validation.

Notable endpoints (examples in repo)

- GET /api/users
- GET /api/users/search?name={name} — returns users whose Name contains the query (case-insensitive).
- POST /api/users
- GET /api/orders — orders are accessible via `OrderRepository` (in-memory).

Build / run / debug

- Build: `dotnet build` at project root. Artifact: `bin/Debug/net9.0/CopilotLabApi.dll`.
- Run: `dotnet run` (uses `launchSettings.json` URLs in development).
- Use `test.http` for quick manual API checks (it contains curl/http examples including the search endpoint).

Integration points & dependencies

- NuGet: `Microsoft.AspNetCore.OpenApi` (Swagger/OpenAPI support). No external databases by default.
- To migrate to a DB: add a DbContext, EF packages, implement a repository backed by the context, and swap the DI registration in `Program.cs`.

Safety rules for automated edits

- Avoid breaking public routes without updating `test.http` and informing the user.
- Keep DI setup centralized in `Program.cs`.
- When changing models, update DTOs, repository create/update calls, and example requests.
- Preserve DataAnnotation validation and return `Results.ValidationProblem(...)` for validation errors.

If anything is unclear (e.g., allowed breaking changes, desired persistence), ask the user before making large structural edits.

End of instructions.

What to change and where (high-value places)

- Add new API endpoints or change routing behavior in `Program.cs` — this file wires DI and routes.
- Change data model fields or validation attributes in `Models/UserModels.cs`.
- Persistency or replace in-memory store: replace `UserRepository` implementation in `Repositories/UserRepository.cs` and register a new repository implementation in `Program.cs`.
- Validation behavior: update `Utils/ValidationHelper.cs` (endpoints call `ValidationHelper.Validate(dto)` and expect an `IResult?`).

Project conventions and patterns

- Minimal API style: use top-level statements in `Program.cs`. Keep routing and DI registrations in this file.
- DTOs use System.ComponentModel.DataAnnotations attributes for validation. Endpoints rely on `ValidationHelper` rather than model-binding automatic validation.
- Repository is registered as a singleton and is thread-safe using a lock and Interlocked for id generation.
- Email uniqueness is enforced at endpoint level via `UserRepository.ExistsByEmail(...)`.

Build / run / debug

- Build: `dotnet build` (project root). Artifact: `bin/Debug/net9.0/CopilotLabApi.dll`.
- Run in development (uses `launchSettings.json` URLs): `dotnet run` (project root).
- Example HTTP requests are in `test.http` and `CopilotLabApi.http` — use them as canonical examples and tests for endpoints.

Testing and safety

- There are currently no unit tests in the repository. If adding tests, prefer xUnit and put tests in a separate `tests/` project.
- Because the repository uses an in-memory singleton store, tests should either isolate the repository instance or create a fresh instance per test.

Integration points and external deps

- NuGet: `Microsoft.AspNetCore.OpenApi` is referenced; otherwise no external services.
- No database or external API integrations are present — migrating to EF Core or external stores requires updating `UserRepository` and DI in `Program.cs`.

Common small change recipes (examples)

- Add a new required field to User (e.g., `Phone`):

  1. Add property to `User` in `Models/UserModels.cs` and to DTOs with appropriate DataAnnotation.
  2. Update repository create/update calls (use `.Trim()` where appropriate).
  3. Update any example requests in `test.http` and adapt validation helper if needed.

- Replace in-memory storage with EF Core (high-level):
  1. Add a `DbContext` in `Data/` and add EF Core packages.
  2. Scaffold `User` entity from DTO or create migration.
  3. Replace `UserRepository` registration in `Program.cs` with `AddDbContext` and register a repository implementation that uses the context.

Style and safety rules for AI edits

- Preserve public API routes and signatures unless user asks for breaking changes. If breaking changes are necessary, update `test.http` and document the change.
- When modifying models, update DTOs and repository logic consistently to avoid nullable/trim errors.
- Keep DI registrations in `Program.cs` and avoid scattering service registration across files.

Files to inspect for context before editing

- `Program.cs` — routing, DI, and app configuration
- `Models/UserModels.cs` — shape and validation rules
- `Repositories/UserRepository.cs` — current storage semantics
- `Utils/ValidationHelper.cs` — how validation results are returned
- `Properties/launchSettings.json` and `test.http` — dev URLs and example requests

If you need clarification

- Ask the user whether you may introduce breaking changes (model, route, DTO) or whether changes must be backwards-compatible.

End of instructions.
