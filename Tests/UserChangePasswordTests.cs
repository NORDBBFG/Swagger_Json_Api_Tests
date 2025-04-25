using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class UserChangePasswordTests
{
    private UserApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new UserApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task ChangePassword_ValidData_ShouldSucceed()
    {
        // Arrange
        var model = new UserChangePasswordModel
        {
            PhoneNumber = "+1234567890",
            OldPassword = "oldPassword123",
            Password = "newPassword456"
        };

        // Act
        var response = await _apiClient.ChangePasswordAsync(model);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Be("Password was successfully updated");
    }

    [Test]
    public async Task ChangePassword_InvalidOldPassword_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new UserChangePasswordModel
        {
            PhoneNumber = "+1234567890",
            OldPassword = "wrongOldPassword",
            Password = "newPassword456"
        };

        // Act
        var response = await _apiClient.ChangePasswordAsync(model);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Invalid old password");
    }

    [Test]
    public async Task ChangePassword_InvalidPhoneNumber_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new UserChangePasswordModel
        {
            PhoneNumber = "invalidPhoneNumber",
            OldPassword = "oldPassword123",
            Password = "newPassword456"
        };

        // Act
        var response = await _apiClient.ChangePasswordAsync(model);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Invalid phone number format");
    }

    [Test]
    public async Task ChangePassword_WeakNewPassword_ShouldReturnBadRequest()
    {
        // Arrange
        var model = new UserChangePasswordModel
        {
            PhoneNumber = "+1234567890",
            OldPassword = "oldPassword123",
            Password = "weak"
        };

        // Act
        var response = await _apiClient.ChangePasswordAsync(model);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Password does not meet complexity requirements");
    }
}
