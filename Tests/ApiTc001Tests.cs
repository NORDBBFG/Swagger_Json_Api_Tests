using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;

[TestFixture]
public class ApiTc001Tests
{
    private HttpClient _httpClient;
    private UserApiClient _userApiClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _userApiClient = new UserApiClient(_httpClient);
    }

    [Test]
    public async Task VerifySuccessfulUserLogin_WithValidCredentials()
    {
        // Arrange
        var loginModel = new
        {
            email = "testuser@example.com",
            password = "ValidPassword123"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/user/login", loginModel);
        var content = await response.Content.ReadAsStringAsync();
        var jsonContent = JObject.Parse(content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        jsonContent["message"].Value<string>().Should().Be("Login successful");
        jsonContent["token"].Value<string>().Should().NotBeNullOrEmpty();
    }

    [TearDown]
    public void Teardown()
    {
        _httpClient.Dispose();
    }
}