using NUnit.Framework;
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
        _apiClient = new FaqApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task GetFaqCategories_ReturnsSuccessfulResponse()
    {
        // Act
        var response = await _apiClient.GetFaqCategoriesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<FaqCategoryDto>>();
    }

    [Test]
    public async Task GetFaqCategories_ReturnsValidData()
    {
        // Act
        var response = await _apiClient.GetFaqCategoriesAsync();

        // Assert
        response.Data.Should().NotBeEmpty();
        foreach (var category in response.Data)
        {
            category.Id.Should().BeGreaterThan(0);
            category.Name.Should().NotBeNullOrEmpty();
            category.FaqArticlesCount.Should().BeGreaterThanOrEqualTo(0);
            category.Created.Should().BeBefore(DateTime.UtcNow);
            category.Updated.Should().BeBefore(DateTime.UtcNow);
        }
    }

    [Test]
    public async Task GetFaqCategories_HandlesServerError()
    {
        // Arrange
        // You might need to mock the HttpClient or use a different approach to simulate a 500 error

        // Act
        var response = await _apiClient.GetFaqCategoriesAsync();

        // Assert
        if (response.StatusCode == 500)
        {
            response.Error.Should().NotBeNull();
            response.Error.StatusCode.Should().Be(500);
            response.Error.Content.Should().NotBeNullOrEmpty();
        }
        else
        {
            Assert.Inconclusive("Unable to test 500 error handling without mocking.");
        }
    }
}