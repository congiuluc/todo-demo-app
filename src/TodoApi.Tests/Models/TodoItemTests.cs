using TodoApi.Models;

namespace TodoApi.Tests.Models;

public class TodoItemTests
{
    [Fact]
    public void TodoItem_CanSetAndGetId()
    {
        // Arrange
        var todoItem = new TodoItem();
        var expectedId = 42;

        // Act
        todoItem.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, todoItem.Id);
    }

    [Fact]
    public void TodoItem_CanSetAndGetTitle()
    {
        // Arrange
        var todoItem = new TodoItem();
        var expectedTitle = "Test Title";

        // Act
        todoItem.Title = expectedTitle;

        // Assert
        Assert.Equal(expectedTitle, todoItem.Title);
    }

    [Fact]
    public void TodoItem_CanSetAndGetDescription()
    {
        // Arrange
        var todoItem = new TodoItem();
        var expectedDescription = "Test Description";

        // Act
        todoItem.Description = expectedDescription;

        // Assert
        Assert.Equal(expectedDescription, todoItem.Description);
    }

    [Fact]
    public void TodoItem_CanSetAndGetIsComplete()
    {
        // Arrange
        var todoItem = new TodoItem();

        // Act
        todoItem.IsComplete = true;

        // Assert
        Assert.True(todoItem.IsComplete);
    }

    [Fact]
    public void TodoItem_DefaultIsCompleteIsFalse()
    {
        // Arrange & Act
        var todoItem = new TodoItem();

        // Assert
        Assert.False(todoItem.IsComplete);
    }

    [Fact]
    public void TodoItem_CanInitializeWithObjectInitializer()
    {
        // Arrange & Act
        var todoItem = new TodoItem
        {
            Id = 1,
            Title = "Test",
            Description = "Description",
            IsComplete = true
        };

        // Assert
        Assert.Equal(1, todoItem.Id);
        Assert.Equal("Test", todoItem.Title);
        Assert.Equal("Description", todoItem.Description);
        Assert.True(todoItem.IsComplete);
    }
}
