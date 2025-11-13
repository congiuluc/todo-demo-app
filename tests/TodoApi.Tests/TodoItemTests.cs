using TodoApi.Models;

namespace TodoApi.Tests;

public class TodoItemTests
{
    [Fact]
    public void TodoItem_CanBeCreated()
    {
        // Arrange & Act
        var todoItem = new TodoItem
        {
            Id = 1,
            Title = "Test Todo",
            Description = "Test Description",
            IsComplete = false
        };

        // Assert
        Assert.Equal(1, todoItem.Id);
        Assert.Equal("Test Todo", todoItem.Title);
        Assert.Equal("Test Description", todoItem.Description);
        Assert.False(todoItem.IsComplete);
    }

    [Fact]
    public void TodoItem_CanBeMarkedComplete()
    {
        // Arrange
        var todoItem = new TodoItem
        {
            Id = 1,
            Title = "Test Todo",
            Description = "Test Description",
            IsComplete = false
        };

        // Act
        todoItem.IsComplete = true;

        // Assert
        Assert.True(todoItem.IsComplete);
    }

    [Fact]
    public void TodoItem_PropertiesCanBeNull()
    {
        // Arrange & Act
        var todoItem = new TodoItem
        {
            Id = 1,
            Title = null,
            Description = null,
            IsComplete = false
        };

        // Assert
        Assert.Null(todoItem.Title);
        Assert.Null(todoItem.Description);
    }
}
