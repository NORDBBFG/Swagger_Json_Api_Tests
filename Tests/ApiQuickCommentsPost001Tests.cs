using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiQuickCommentsPost001Tests
{
    private QuickCommentApiClient _client;
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer {valid_token}");
        _client = new QuickCommentApiClient(_httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();

        foreach (var comment in response.Data)
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorContent.Should().NotBeNull();
        response.ErrorContent.StatusCode.Should().Be(400);
        response.ErrorContent.Content.Should().NotBeNullOrEmpty();
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}