using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;

[TestFixture]
public class ApiQc001Tests
{
    private HttpClient _httpClient;
    private QuickCommentApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new System.Uri("https://api-base-url.com") // Replace with actual base URL
        };
        _apiClient = new QuickCommentApiClient(_httpClient);
    }

    [Test]
    public async Task GetQuickComments_ValidCategory_ReturnsQuickComments()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<QuickCommentDto>>();

        foreach (var quickComment in response.Data)
        {
            quickComment.Id.Should().BeGreaterThan(0);
            quickComment.Comment.Should().NotBeNull().When(comment => comment != null);
        }

        _httpClient.DefaultRequestHeaders.Accept
            .Should().Contain(header => header.MediaType == "application/json");
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)999; // Invalid category

        // Act
        var response = await _apiClient.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}
