using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiV1UserFaqsPostTests
{
    private ApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new ApiClient();
    }

    [Test]
    public async Task FavoriteFaqs_ReturnsValidContentResult()
    {
        // Arrange
        int[] faqIds = new int[] { 1, 2, 3 };

        // Act
        var response = await _apiClient.FavoriteFaqsAsync(faqIds);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<ContentResult>();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task FavoriteFaqs_WithInvalidInput_ReturnsBadRequest()
    {
        // Arrange
        int[] invalidFaqIds = new int[] { -1, 0, 999999 }; // Assuming these are invalid IDs

        // Act
        var response = await _apiClient.FavoriteFaqsAsync(invalidFaqIds);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<ContentResult>();
        response.Data.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task FavoriteFaqs_WithEmptyArray_ReturnsValidResponse()
    {
        // Arrange
        int[] emptyFaqIds = new int[] { };

        // Act
        var response = await _apiClient.FavoriteFaqsAsync(emptyFaqIds);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<ContentResult>();
    }
}
