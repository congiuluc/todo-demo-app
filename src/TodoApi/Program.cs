using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Always enable OpenAPI/Swagger for both development and release
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable OpenAPI/Swagger for both development and release
app.MapOpenApi();

app.UseHttpsRedirection();

// In-memory storage for Todo items
var todoItems = new List<TodoItem>
{
    new TodoItem { Id = 1, Title = "Learn .NET Minimal APIs", Description = "Study the documentation", IsComplete = false },
    new TodoItem { Id = 2, Title = "Build a Todo API", Description = "Create a simple API with CRUD operations", IsComplete = false }
};

// Get all Todo items
app.MapGet("/todos", () => todoItems)
    .WithName("GetAllTodos")
    .WithDescription("Retrieves all todo items")
    .WithOpenApi();

// Get a specific Todo item by ID
app.MapGet("/todos/{id}", (int id) =>
{
    var todo = todoItems.FirstOrDefault(t => t.Id == id);
    return todo is null ? Results.NotFound() : Results.Ok(todo);
})
.WithName("GetTodoById")
.WithDescription("Retrieves a specific todo item by ID")
.WithOpenApi();

// Create a new Todo item
app.MapPost("/todos", (TodoItem todo) =>
{
    var newId = todoItems.Count > 0 ? todoItems.Max(t => t.Id) + 1 : 1;
    todo.Id = newId;
    todoItems.Add(todo);
    return Results.Created($"/todos/{todo.Id}", todo);
})
.WithName("CreateTodo")
.WithDescription("Creates a new todo item")
.WithOpenApi();

// Update a Todo item
app.MapPut("/todos/{id}", (int id, TodoItem updatedTodo) =>
{
    var index = todoItems.FindIndex(t => t.Id == id);
    if (index == -1) return Results.NotFound();
    
    updatedTodo.Id = id;
    todoItems[index] = updatedTodo;
    return Results.NoContent();
})
.WithName("UpdateTodo")
.WithDescription("Updates an existing todo item")
.WithOpenApi();

// Delete a Todo item
app.MapDelete("/todos/{id}", (int id) =>
{
    var index = todoItems.FindIndex(t => t.Id == id);
    if (index == -1) return Results.NotFound();
    
    todoItems.RemoveAt(index);
    return Results.NoContent();
})
.WithName("DeleteTodo")
.WithDescription("Deletes a todo item")
.WithOpenApi();

app.Run();

// Make the implicit Program class public for testing
public partial class Program { }
