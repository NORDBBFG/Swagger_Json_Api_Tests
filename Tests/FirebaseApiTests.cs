using System;
using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;

[TestFixture]
public class FirebaseApiTests
{
    private FirebaseApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new FirebaseApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task EditPushNotificationCategory_ValidData_ReturnsSuccessResult()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            Id = 1,
            Name = "Test Category",
            Picture = "https://example.com/image.jpg",
            PushNotificationsCount = 5,
            CreatedById = "user1",
            LastUpdatedById = "user2",
            Created = DateTime.UtcNow.AddDays(-1),
            Updated = DateTime.UtcNow,
            IsDisabled = false
        };

        // Act
        var result = await _client.EditPushNotificationCategoryAsync(category);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task EditPushNotificationCategory_InvalidData_ReturnsBadRequestResult()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            Id = -1, // Invalid ID
            Name = "", // Empty name
            Picture = "not a valid url",
            PushNotificationsCount = -5, // Negative count
            CreatedById = null,
            LastUpdatedById = null,
            Created = DateTime.UtcNow.AddDays(1), // Future date
            Updated = DateTime.UtcNow.AddDays(-1), // Past date
            IsDisabled = false
        };

        // Act
        var result = await _client.EditPushNotificationCategoryAsync(category);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(400);
        result.Content.Should().Contain("Bad request");
    }

    [Test]
    public async Task EditPushNotificationCategory_ServerError_ReturnsInternalServerErrorResult()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            Id = 999999, // Assuming this ID causes a server error
            Name = "Error Category",
            Picture = "https://example.com/error.jpg",
            PushNotificationsCount = 0,
            CreatedById = "user1",
            LastUpdatedById = "user1",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            IsDisabled = true
        };

        // Act
        var result = await _client.EditPushNotificationCategoryAsync(category);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(500);
        result.Content.Should().Contain("Internal server error");
    }
}