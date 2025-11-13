using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi.Models;

namespace TodoApi.Tests;

public class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
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
    public async Task GetAllTodos_ReturnsExpectedContent()
    {
        // Act
        var response = await _client.GetAsync("/todos");
        var todos = await response.Content.ReadFromJsonAsync<List<TodoItem>>();

        // Assert
        Assert.NotNull(todos);
        Assert.NotEmpty(todos);
        Assert.True(todos.Count >= 2); // At least the default todos in Program.cs
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
    public async Task GetTodoById_WithValidId_ReturnsExpectedTodo()
    {
        // Act
        var response = await _client.GetAsync("/todos/1");
        var todo = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        Assert.NotNull(todo);
        Assert.Equal(1, todo.Id);
        Assert.NotNull(todo.Title); // Title exists (may have been updated by other tests)
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
            Title = "New Todo",
            Description = "New Description",
            IsComplete = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/todos", newTodo);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateTodo_WithValidData_ReturnsCreatedTodo()
    {
        // Arrange
        var newTodo = new TodoItem
        {
            Title = "Another Todo",
            Description = "Another Description",
            IsComplete = false
        };

        // Act
        var response = await _client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await response.Content.ReadFromJsonAsync<TodoItem>();

        // Assert
        Assert.NotNull(createdTodo);
        Assert.Equal("Another Todo", createdTodo.Title);
        Assert.Equal("Another Description", createdTodo.Description);
        Assert.False(createdTodo.IsComplete);
        Assert.True(createdTodo.Id > 0);
    }

    [Fact]
    public async Task UpdateTodo_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var updatedTodo = new TodoItem
        {
            Title = "Updated Todo",
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
            Title = "Updated Todo",
            Description = "Updated Description",
            IsComplete = true
        };

        // Act
        var response = await _client.PutAsJsonAsync("/todos/999", updatedTodo);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WithValidId_ReturnsNoContent()
    {
        // Arrange
        // First create a todo to delete
        var newTodo = new TodoItem
        {
            Title = "Todo to Delete",
            Description = "This will be deleted",
            IsComplete = false
        };
        var createResponse = await _client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/todos/{createdTodo!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
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
    public async Task DeleteTodo_VerifyTodoIsDeleted()
    {
        // Arrange
        // First create a todo to delete
        var newTodo = new TodoItem
        {
            Title = "Todo to Delete and Verify",
            Description = "This will be deleted and verified",
            IsComplete = false
        };
        var createResponse = await _client.PostAsJsonAsync("/todos", newTodo);
        var createdTodo = await createResponse.Content.ReadFromJsonAsync<TodoItem>();

        // Act
        await _client.DeleteAsync($"/todos/{createdTodo!.Id}");
        var getResponse = await _client.GetAsync($"/todos/{createdTodo.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
