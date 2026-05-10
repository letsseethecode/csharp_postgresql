using Xunit;

using LSTC.Shared.CQS.Events;
using LSTC.Shared.Domain;

namespace LSTC.Shared.Tests.Domain;

public class ResultTests
{
    public class TestEvent : IEvent
    {
        public string Name { get; }

        public TestEvent(string name)
        {
            Name = name;
        }
    }

    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        // Act
        var result = Result.Success("Test");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test", result.Value);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void Failure_ShouldCreateFailedResult()
    {
        // Act
        var result = Result<string>.Failure("Error occurred");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal("Error occurred", result.ErrorMessage);
    }

    [Fact]
    public void WithEvents_ShouldAddEventsToResult()
    {
        // Arrange
        var result = Result.Success("Test");

        var event1 = new TestEvent("Event 1");
        var event2 = new TestEvent("Event 2");

        // Act
        var updatedResult = result.WithEvents(event1, event2);

        // Assert
        Assert.Contains(event1, updatedResult.Events);
        Assert.Contains(event2, updatedResult.Events);
    }
}