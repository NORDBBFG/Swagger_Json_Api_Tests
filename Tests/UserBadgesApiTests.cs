using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;

[TestFixture]
public class UserBadgesApiTests
{
    private UserBadgesApiClient _client;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _client = new UserBadgesApiClient(_httpClient);
    }

    [Test]
    public async Task AT157_GetUserBadges_SuccessfulRetrieval()
    {
        // Arrange
        // Assuming we have a valid authentication mechanism
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "validToken");

        // Act
        var response = await _client.GetUserBadgesAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<BadgeDto>>();
        response.Data.Should().NotBeEmpty();

        // Validate structure of BadgeDto
        var firstBadge = response.Data[0];
        firstBadge.Should().NotBeNull();
        firstBadge.Id.Should().BeGreaterThan(0);
        firstBadge.Name.Should().NotBeNullOrEmpty();
        firstBadge.CreateDate.Should().NotBe(default(DateTime));
        firstBadge.UpdateDate.Should().NotBe(default(DateTime));
    }

    [Test]
    public async Task AT158_GetUserBadges_UnauthorizedAccess()
    {
        // Arrange
        // Ensure no authentication token is set
        _httpClient.DefaultRequestHeaders.Authorization = null;

        // Act
        var response = await _client.GetUserBadgesAsync();

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
        response.ErrorContent.Content.Should().Contain("Unauthorized");
    }

    [TearDown]
    public void Teardown()
    {
        _httpClient.Dispose();
    }
}