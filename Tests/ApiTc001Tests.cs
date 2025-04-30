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
    public async Task CreateUser_ValidData_ReturnsCreatedStatus()
    {
        // Arrange
        var createUserModel = new CreateUserModel
        {
            Name = "John Doe",
            Email = "johndoe@example.com",
            Password = "SecurePassword123"
        };

        // Act
        var response = await _client.CreateUserAsync(createUserModel);

        // Assert
        response.StatusCode.Should().Be(201);
        response.Content.Should().NotBeNull();
        response.Content.Name.Should().Be("John Doe");
        response.Content.Email.Should().Be("johndoe@example.com");
        response.Content.Id.Should().NotBeNullOrEmpty();
    }
}

// Note: This test case assumes the existence of a CreateUserModel and a CreateUserAsync method,
// which are not present in the provided Swagger fragment. You may need to adjust the test
// based on the actual API structure for creating users.
