using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class FaqCategoriesApiTests
{
    private FaqApiClient _apiClient;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _apiClient = new FaqApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task AT_150_GetFaqCategories_SuccessfulRetrieval()
    {
        // Arrange

        // Act
        var response = await _apiClient.GetFaqCategoriesAsync();

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<FaqCategoryDto>>();
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
    public async Task AT_151_GetFaqCategories_ServerError()
    {
        // Arrange
        // Mock the API to return a 500 error (you may need to use a mocking framework or set up a test server)

        // Act
        var response = await _apiClient.GetFaqCategoriesAsync();

        // Assert
        response.StatusCode.Should().Be(500);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(500);
        response.ErrorResult.Content.Should().NotBeNullOrEmpty();
    }
}