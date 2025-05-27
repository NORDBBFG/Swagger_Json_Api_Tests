using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;

[TestFixture]
public class FaqCategoriesTests
{
    private FaqApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _apiClient = new FaqApiClient(httpClient);
    }

    [Test]
    public async Task AT_152_ValidateSuccessfulRetrievalOfFaqCategories()
    {
        // Arrange
        // No additional arrangement needed

        // Act
        var response = await _apiClient.GetFaqCategoriesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<FaqCategoryDto>>();
        response.Data.Should().NotBeEmpty();

        // Validate structure and key fields of the first category
        var firstCategory = response.Data.First();
        firstCategory.Should().NotBeNull();
        firstCategory.Id.Should().BeGreaterThan(0);
        firstCategory.Name.Should().NotBeNullOrEmpty();
        firstCategory.FaqArticlesCount.Should().BeGreaterThanOrEqualTo(0);
        firstCategory.Created.Should().BeBefore(DateTime.UtcNow);
        firstCategory.Updated.Should().BeBefore(DateTime.UtcNow);
    }

    [Test]
    public async Task AT_153_ValidateServerErrorWhenRetrievingFaqCategories()
    {
        // Arrange
        // For this test, we need to mock a server error.
        // In a real scenario, you might use a mocking framework or a test API that can return 500 errors.
        // For this example, we'll assume the API client has been modified to throw an exception for testing purposes.

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await _apiClient.GetFaqCategoriesAsync());

        // Alternative approach if the API client returns a response with error details:
        /*
        var response = await _apiClient.GetFaqCategoriesAsync();
        response.StatusCode.Should().Be(500);
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        */
    }
}