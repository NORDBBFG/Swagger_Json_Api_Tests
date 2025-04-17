using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiPostFirebase001Tests
{
    private FirebaseApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new FirebaseApiClient(httpClient);
    }

    [Test]
    public async Task SavePushToken_WithValidData_ShouldReturnSuccessResponse()
    {
        // Arrange
        string deviceId = "12345-abcde";
        string token = "validPushToken123";

        // Act
        var response = await _client.SavePushTokenAsync(deviceId, token);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Be("PushToken was saved successfully");
        response.Data.ContentType.Should().Be("application/json");
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task SavePushToken_WithInvalidData_ShouldReturnBadRequestResponse()
    {
        // Arrange
        string deviceId = "";
        string token = "";

        // Act
        var response = await _client.SavePushTokenAsync(deviceId, token);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Bad request");
    }

    [TearDown]
    public void Teardown()
    {
        // Add any necessary cleanup code here
    }
}