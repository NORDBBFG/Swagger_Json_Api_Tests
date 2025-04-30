using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiUserPut001Tests
{
    private UserApiClient _userApiClient;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _userApiClient = new UserApiClient(httpClient);
    }

    [Test]
    public async Task ChangePassword_WithValidModel_ShouldReturnSuccessResponse()
    {
        // Arrange
        var changePasswordModel = new UserChangePasswordModel
        {
            OldPassword = "oldPassword123",
            Password = "newPassword456",
            PhoneNumber = "+1234567890"
        };

        // Act
        var response = await _userApiClient.ChangePasswordAsync(changePasswordModel);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.StatusCode.Should().Be(200);
        response.Data.Content.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task ChangePassword_WithInvalidModel_ShouldReturnBadRequestResponse()
    {
        // Arrange
        var changePasswordModel = new UserChangePasswordModel
        {
            OldPassword = "",
            Password = "",
            PhoneNumber = ""
        };

        // Act
        var response = await _userApiClient.ChangePasswordAsync(changePasswordModel);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.StatusCode.Should().Be(400);
        response.Data.Content.Should().NotBeNullOrEmpty();
    }
}