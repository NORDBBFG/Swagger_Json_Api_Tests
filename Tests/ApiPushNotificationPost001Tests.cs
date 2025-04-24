using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiPushNotificationPost001Tests
{
    private PushNotificationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new PushNotificationApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task CreatePushNotification_ValidPayload_ReturnsSuccessfulResponse()
    {
        // Arrange
        var pushNotification = new PushNotificationDto
        {
            Name = "Test Notification",
            CategoryId = 1,
            ShortDescription = "Short description",
            LongDescription = "Long description",
            NotificationDistribution = DateTime.UtcNow.AddDays(1),
            CreatedById = "user1",
            LastUpdatedById = "user1",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            IsDisabled = false
        };

        // Act
        var response = await _client.CreatePushNotificationAsync(pushNotification);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.StatusCode.Should().Be(200);
        response.Data.Content.Should().NotBeNullOrEmpty();
    }
}