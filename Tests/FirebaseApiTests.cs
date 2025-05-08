using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class FirebaseApiTests
{
    private FirebaseApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new FirebaseApiClient(httpClient);
    }

    [Test]
    public async Task CreatePushNotificationCategory_ValidData_ReturnsSuccessResult()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            Name = "Test Category",
            Description = "This is a test category"
        };

        // Act
        var response = await _client.CreatePushNotificationCategoryAsync(category);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.StatusCode.Should().Be(200);
        response.Data.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task CreatePushNotificationCategory_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            // Invalid data: missing Name
            Description = "This is an invalid category"
        };

        // Act
        var response = await _client.CreatePushNotificationCategoryAsync(category);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(400);
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task EditPushNotificationCategory_ValidData_ReturnsSuccessResult()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            Id = 1, // Assuming this ID exists
            Name = "Updated Category",
            Description = "This is an updated category"
        };

        // Act
        var response = await _client.EditPushNotificationCategoryAsync(category);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.StatusCode.Should().Be(200);
        response.Data.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task EditPushNotificationCategory_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        var category = new PushNotificationCategoryDto
        {
            Id = -1, // Invalid ID
            Name = "Invalid Category",
            Description = "This is an invalid category"
        };

        // Act
        var response = await _client.EditPushNotificationCategoryAsync(category);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(400);
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }
}