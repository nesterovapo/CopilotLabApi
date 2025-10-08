using CopilotLabApi.Models;
using CopilotLabApi.Repositories;
using CopilotLabApi.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// In-memory repositories registered as singletons
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<OrderRepository>();
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddSingleton<CategoryRepository>();
builder.Services.AddSingleton<ContactRepository>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// --- Users API (In-memory CRUD) ---

app.MapGet("/api/users", (UserRepository repo) => Results.Ok(repo.GetAll()))
   .WithName("GetUsers");

app.MapGet("/api/users/search", (string? name, UserRepository repo) =>
{
    if (string.IsNullOrWhiteSpace(name)) return Results.BadRequest(new { error = "name query parameter is required" });
    var results = repo.SearchByName(name);
    return Results.Ok(results);
})
    .WithName("SearchUsersByName");

app.MapGet("/api/users/{id:int}", (int id, UserRepository repo) =>
{
    var user = repo.Get(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
})
   .WithName("GetUserById");

app.MapPost("/api/users", (UserCreateDto dto, UserRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    // unique email check
    if (repo.ExistsByEmail(dto.Email!))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["Email"] = new[] { "Email must be unique." }
        });
    }

    var user = repo.Create(dto);
    return Results.Created($"/api/users/{user.Id}", user);
})
    .WithName("CreateUser");

app.MapPut("/api/users/{id:int}", (int id, UserUpdateDto dto, UserRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    var existing = repo.Get(id);
    if (existing is null) return Results.NotFound();

    // unique email check (exclude current user)
    if (repo.ExistsByEmail(dto.Email!, excludeId: id))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["Email"] = new[] { "Email must be unique." }
        });
    }

    var updated = repo.Update(id, dto);
    return updated ? Results.NoContent() : Results.NotFound();
})
   .WithName("UpdateUser");

app.MapDelete("/api/users/{id:int}", (int id, UserRepository repo) =>
{
    var removed = repo.Delete(id);
    return removed ? Results.NoContent() : Results.NotFound();
})
   .WithName("DeleteUser");

// --- Orders API (In-memory) ---

app.MapGet("/api/orders", (OrderRepository repo) => Results.Ok(repo.GetAll()))
    .WithName("GetOrders");

app.MapGet("/api/orders/{id:int}", (int id, OrderRepository repo) =>
{
    var order = repo.GetById(id);
    return order is null ? Results.NotFound() : Results.Ok(order);
})
    .WithName("GetOrderById");

app.MapPost("/api/orders", (OrderCreateDto dto, OrderRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    // In a real system we'd check that the UserId exists; keep simple for in-memory demo
    var created = repo.Create(dto);
    return Results.Created($"/api/orders/{created.Id}", created);
})
    .WithName("CreateOrder");

// --- Categories API (In-memory CRUD) ---

app.MapGet("/api/categories", (CategoryRepository repo) => Results.Ok(repo.GetAll()))
    .WithName("GetCategories");

app.MapGet("/api/categories/{id:int}", (int id, CategoryRepository repo) =>
{
    var c = repo.GetById(id);
    return c is null ? Results.NotFound() : Results.Ok(c);
})
    .WithName("GetCategoryById");

app.MapPost("/api/categories", (CategoryCreateDto dto, CategoryRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    var created = repo.Create(dto);
    return Results.Created($"/api/categories/{created.Id}", created);
})
    .WithName("CreateCategory");

app.MapPut("/api/categories/{id:int}", (int id, CategoryUpdateDto dto, CategoryRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    var ok = repo.Update(id, dto);
    return ok ? Results.NoContent() : Results.NotFound();
})
    .WithName("UpdateCategory");

app.MapDelete("/api/categories/{id:int}", (int id, CategoryRepository repo) =>
{
    var ok = repo.Delete(id);
    return ok ? Results.NoContent() : Results.NotFound();
})
    .WithName("DeleteCategory");

// --- Products API (In-memory CRUD) ---

app.MapGet("/api/products", (ProductRepository repo) => Results.Ok(repo.GetAll()))
    .WithName("GetProducts");

app.MapPost("/api/products", (ProductCreateDto dto, ProductRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    var product = repo.Create(dto);
    return Results.Created($"/api/products/{product.Id}", product);
})
    .WithName("CreateProduct");

app.MapPut("/api/products/{id:int}", (int id, ProductUpdateDto dto, ProductRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    var updated = repo.Update(id, dto);
    return updated ? Results.NoContent() : Results.NotFound();
})
    .WithName("UpdateProduct");

app.MapDelete("/api/products/{id:int}", (int id, ProductRepository repo) =>
{
    var removed = repo.Delete(id);
    return removed ? Results.NoContent() : Results.NotFound();
})
    .WithName("DeleteProduct");

// --- Contact API ---
app.MapPost("/api/contact", (ContactCreateDto dto, ContactRepository repo) =>
{
    var validation = ValidationHelper.Validate(dto);
    if (validation != null) return validation;

    if (!ValidationHelper.IsValidEmail(dto.Email))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["Email"] = new[] { "Email is not valid." }
        });
    }

    var created = repo.Create(dto);
    return Results.Created($"/api/contact/{created.Id}", created);
})
    .WithName("CreateContact");

app.Run();
