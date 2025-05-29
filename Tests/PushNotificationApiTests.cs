using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class PushNotificationApiTests
{
    private FirebaseApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new FirebaseApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task AT155_CreatePushNotification_SuccessfulCreation()
    {
        // Arrange
        var notification = new PushNotificationDto
        {
            Title = "Test Notification",
            Body = "This is a test notification",
            Topic = "TestTopic",
            ScheduledTime = DateTime.UtcNow.AddHours(1)
        };

        // Act
        var response = await _client.CreatePushNotificationAsync(notification);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AT156_CreatePushNotification_InvalidPayload()
    {
        // Arrange
        var invalidNotification = new PushNotificationDto
        {
            // Missing required fields
        };

        // Act
        var response = await _client.CreatePushNotificationAsync(invalidNotification);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Bad request");
        response.Data.StatusCode.Should().Be(400);
    }
}