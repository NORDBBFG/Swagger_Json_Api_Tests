using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiEndpointDetails001Tests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationApiClient("https://api-base-url.com"); // Replace with actual base URL
    }

    [Test]
    public async Task GetQuickComments_ValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var category = QuickCommentCategory.ReasonForCancellation;

        // Act
        var response = await _client.GetQuickCommentsAsync(category);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<QuickCommentDto>>();
        
        foreach (var comment in response.Data)
        {
            comment.Id.Should().BeGreaterThan(0);
            comment.Comment.Should().NotBeNullOrEmpty();
        }

        response.ErrorResult.Should().BeNull();
    }

    [Test]
    public async Task GetQuickComments_InvalidCategory_ReturnsBadRequest()
    {
        // Arrange
        var invalidCategory = (QuickCommentCategory)100; // Invalid category

        // Act
        var response = await _client.GetQuickCommentsAsync(invalidCategory);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
        response.ErrorResult.Content.Should().NotBeNullOrEmpty();
    }
}