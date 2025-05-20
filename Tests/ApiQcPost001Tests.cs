using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiQcPost001Tests
{
    private HttpClient _httpClient;
    private QuickCommentsApiClient _apiClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _apiClient = new QuickCommentsApiClient(_httpClient, BaseUrl);
    }

    [Test]
    public async Task CreateQuickComment_ReturnsCollectionOfQuickComments()
    {
        // Arrange
        var request = new QuickCommentRequestDto
        {
            Comment = "Test comment",
            UserId = 1
        };

        // Act
        var response = await _apiClient.CreateQuickCommentAsync(request);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();

        foreach (var quickComment in response.Data)
        {
            quickComment.Should().NotBeNull();
            quickComment.Id.Should().BeGreaterThan(0);
            quickComment.Comment.Should().NotBeNullOrEmpty();
            quickComment.UserId.Should().BeGreaterThan(0);
            quickComment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}
