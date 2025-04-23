using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiFaqsCategoryPost001Tests
{
    private FaqCategoryApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new FaqCategoryApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task AddFaqCategory_ValidPayload_ReturnsSuccessfulResponse()
    {
        // Arrange
        var faqCategory = new FaqCategoryDto
        {
            Name = "Test Category",
            Picture = "https://example.com/test.jpg",
            FaqArticlesCount = 0,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            CreatedBy = "TestUser",
            LastUpdatedBy = "TestUser",
            CreatedByName = "Test User",
            LastUpdatedByName = "Test User",
            IsPermanent = false
        };

        // Act
        var response = await _apiClient.AddFaqCategoryAsync(faqCategory);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(faqCategory, options => options
            .Excluding(o => o.Id)
            .Excluding(o => o.Created)
            .Excluding(o => o.Updated));
        response.Data.Id.Should().BeGreaterThan(0);
        response.Data.Created.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        response.Data.Updated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}