using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiFaqPut001Tests
{
    private FaqApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _apiClient = new FaqApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task EditFaqCategory_ValidData_ReturnsUpdatedFaqCategory()
    {
        // Arrange
        var faqCategory = new FaqCategoryDto
        {
            Id = 1,
            Name = "Updated FAQ Category",
            Picture = "https://example.com/updated-picture.jpg",
            FaqArticlesCount = 5,
            Created = DateTime.UtcNow.AddDays(-1),
            Updated = DateTime.UtcNow,
            CreatedBy = "John Doe",
            LastUpdatedBy = "Jane Smith",
            CreatedByName = "John Doe",
            LastUpdatedByName = "Jane Smith",
            IsPermanent = false
        };

        // Act
        var response = await _apiClient.EditFaqCategoryAsync(faqCategory);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(faqCategory, options => options
            .Excluding(f => f.Updated)
            .Excluding(f => f.LastUpdatedBy)
            .Excluding(f => f.LastUpdatedByName));
        response.Data.Updated.Should().BeAfter(faqCategory.Updated);
        response.Data.LastUpdatedBy.Should().NotBeNullOrEmpty();
        response.Data.LastUpdatedByName.Should().NotBeNullOrEmpty();
    }
}