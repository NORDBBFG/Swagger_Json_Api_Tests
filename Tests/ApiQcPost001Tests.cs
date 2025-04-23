using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;

[TestFixture]
public class ApiQcPost001Tests
{
    private HttpClient _httpClient;
    private QuickCommentApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new System.Uri("https://api-base-url.com"); // Replace with actual base URL
        _apiClient = new QuickCommentApiClient(_httpClient);
    }

    [Test]
    public async Task GetQuickComments_WithValidCategory_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsByCategoryAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull().And.BeOfType<List<QuickCommentDto>>();
        response.Data.Should().NotBeEmpty();

        foreach (var quickComment in response.Data)
        {
            quickComment.Id.Should().BeGreaterThan(0);
            quickComment.Comment.Should().NotBeNull();
        }

        // Verify content type (this would typically be done at the HttpClient level, 
        // but we're assuming it here based on the API client implementation)
        _httpClient.DefaultRequestHeaders.Accept.Should().Contain(header => header.MediaType == "application/json");
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}