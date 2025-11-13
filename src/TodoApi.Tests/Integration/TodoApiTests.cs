using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi.Models;

namespace TodoApi.Tests.Integration;

public class TodoApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TodoApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllTodos_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/todos");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllTodos_ReturnsListOfTodoItems()
    {
        // Act
        var todos = await _client.GetFromJsonAsync<List<TodoItem>>("/todos");

        // Assert
        Assert.NotNull(todos);
        Assert.NotEmpty(todos);
        Assert.True(todos.Count >= 2, $"Expected at least 2 todos, but got {todos.Count}");
    }

    [Fact]
    public async Task GetAllTodos_ReturnsExpectedInitialData()
    {
        // Act
        var todos = await _client.GetFromJsonAsync<List<TodoItem>>("/todos");

        // Assert
        Assert.NotNull(todos);
        Assert.Contains(todos, t => t.Title == "Learn .NET Minimal APIs");
    }

    [Fact]
    public async Task GetTodoById_WithValidId_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/todos/1");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTodoById_WithValidId_ReturnsTodoItem()
    {
        // Act
        var todo = await _client.GetFromJsonAsync<TodoItem>("/todos/1");

        // Assert
        Assert.NotNull(todo);
        Assert.Equal(1, todo.Id);
        Assert.Equal("Learn .NET Minimal APIs", todo.Title);
    }

    [Fact]
    public async Task GetTodoById_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/todos/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateTodo_WithValidData_ReturnsCreatedStatusCode()
    {
        // Arrange
        var newTodo = new TodoItem
        {
            Title = "New Task",
            Description = "New Description",
            IsComplete = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/todos", newTodo);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateTodo_WithValidData_ReturnsCreatedTodoWithId()
    {
        // Arrange
        var newTodo = new TodoItem
        {
            Title = "Another Task",
            Description = "Another Description",
            IsComplete = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        Assert.NotNull(createdTodo);
        Assert.True(createdTodo.Id > 0);
        Assert.Equal(newTodo.Title, createdTodo.Title);
        Assert.Equal(newTodo.Description, createdTodo.Description);
        Assert.Equal(newTodo.IsComplete, createdTodo.IsComplete);
    }

    [Fact]
    public async Task CreateTodo_ReturnsLocationHeader()
    {
        // Arrange
        var newTodo = new TodoItem
        {
            Title = "Task with Location",
            Description = "Test Location Header",
            IsComplete = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        Assert.NotNull(response.Headers.Location);
        Assert.Contains($"/todos/{createdTodo?.Id}", response.Headers.Location.ToString());
    }

    [Fact]
    public async Task UpdateTodo_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var updatedTodo = new TodoItem
        {
            Id = 1, // Will be overridden by the route parameter
            Title = "Updated Title",
            Description = "Updated Description",
            IsComplete = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/todos/1", updatedTodo);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTodo_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updatedTodo = new TodoItem
        {
            Title = "Updated Title",
            Description = "Updated Description",
            IsComplete = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/todos/999", updatedTodo);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateTodo_UpdatesPersistsData()
    {
        // Arrange
        var client = _factory.CreateClient();
        var updatedTodo = new TodoItem
        {
            Title = "Completely Updated",
            Description = "New Description",
            IsComplete = true
        };

        // Act
        await client.PutAsJsonAsync("/todos/1", updatedTodo);
        var retrievedTodo = await client.GetFromJsonAsync<TodoItem>("/todos/1");

        // Assert
        Assert.NotNull(retrievedTodo);
        Assert.Equal("Completely Updated", retrievedTodo.Title);
        Assert.Equal("New Description", retrievedTodo.Description);
        Assert.True(retrievedTodo.IsComplete);
    }

    [Fact]
    public async Task DeleteTodo_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // First create a todo to delete
        var newTodo = new TodoItem
        {
            Title = "To Be Deleted",
            Description = "This will be removed",
            IsComplete = false
        };
        var createResponse = await client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        var response = await client.DeleteAsync($"/todos/{createdTodo?.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/todos/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_RemovesItemFromList()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // First create a todo to delete
        var newTodo = new TodoItem
        {
            Title = "Will Be Gone",
            Description = "Test deletion",
            IsComplete = false
        };
        var createResponse = await client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        await client.DeleteAsync($"/todos/{createdTodo?.Id}");
        var getResponse = await client.GetAsync($"/todos/{createdTodo?.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateTodo_GeneratesSequentialIds()
    {
        // Arrange
        var client = _factory.CreateClient();
        var todo1 = new TodoItem { Title = "Task 1", Description = "Desc 1" };
        var todo2 = new TodoItem { Title = "Task 2", Description = "Desc 2" };

        // Act
        var response1 = await client.PostAsJsonAsync("/todos", todo1);
        var created1 = await response1.Content.ReadFromJsonAsync<TodoItem>();
        
        var response2 = await client.PostAsJsonAsync("/todos", todo2);
        var created2 = await response2.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        Assert.NotNull(created1);
        Assert.NotNull(created2);
        Assert.True(created2.Id > created1.Id);
    }

    [Fact]
    public async Task GetTodoById_AfterUpdate_ReturnsUpdatedData()
    {
        // Arrange
        var client = _factory.CreateClient();
        var updateData = new TodoItem
        {
            Title = "Modified Title",
            Description = "Modified Description",
            IsComplete = true
        };

        // Act
        await client.PutAsJsonAsync("/todos/2", updateData);
        var todo = await client.GetFromJsonAsync<TodoItem>("/todos/2");

        // Assert
        Assert.NotNull(todo);
        Assert.Equal(2, todo.Id);
        Assert.Equal("Modified Title", todo.Title);
        Assert.Equal("Modified Description", todo.Description);
        Assert.True(todo.IsComplete);
    }
}
