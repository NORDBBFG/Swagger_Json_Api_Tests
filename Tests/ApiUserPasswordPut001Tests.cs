using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiUserPasswordPut001Tests
{
    private UserApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new UserApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task ChangePassword_WithValidData_ShouldReturnSuccessResponse()
    {
        // Arrange
        var changePasswordModel = new UserChangePasswordModel
        {
            OldPassword = "oldPassword123",
            Password = "newPassword456",
            PhoneNumber = "+1234567890"
        };

        // Act
        var response = await _client.ChangePasswordAsync(changePasswordModel);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Password was successfully updated");
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task ChangePassword_WithInvalidData_ShouldReturnBadRequestResponse()
    {
        // Arrange
        var changePasswordModel = new UserChangePasswordModel
        {
            OldPassword = "wrongOldPassword",
            Password = "newPassword456",
            PhoneNumber = "+1234567890"
        };

        // Act
        var response = await _client.ChangePasswordAsync(changePasswordModel);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Bad request");
        response.Data.StatusCode.Should().Be(400);
    }
}