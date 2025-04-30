using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiTc001Tests
{
    private UserApiClient _client;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        _client = new UserApiClient(BaseUrl);
    }

    [Test]
    public async Task ChangePassword_ValidInput_ReturnsSuccessfulResponse()
    {
        // Arrange
        var model = new UserChangePasswordModel
        {
            OldPassword = "oldPassword123",
            Password = "newPassword456",
            PhoneNumber = "+1234567890"
        };

        // Act
        var response = await _client.ChangePasswordAsync(model);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task CreatePassword_ValidInput_ReturnsSuccessfulResponse()
    {
        // Arrange
        var model = new CreatePasswordViewModel
        {
            Password = "newPassword789",
            Email = "user@example.com"
        };

        // Act
        var response = await _client.CreatePasswordAsync(model);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }
}
