using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiFaqsCategoryPostPutTests
{
    private FaqCategoryApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _apiClient = new FaqCategoryApiClient(httpClient);
    }

    [Test]
    public async Task API_FAQS_CATEGORY_POST_PUT_Test()
    {
        // Step 1: Send a POST request to /api/v1/faqs/category with a valid FaqCategoryDto payload.
        var newCategory = new FaqCategoryDto
        {
            Name = "Test Category",
            Description = "This is a test category"
        };

        var postResponse = await _apiClient.AddFaqCategoryAsync(newCategory);

        // Step 2: Verify the response status is 200 and the response body matches the FaqCategoryDto schema.
        postResponse.StatusCode.Should().Be(200);
        postResponse.IsSuccessStatusCode.Should().BeTrue();
        postResponse.Data.Should().NotBeNull();
        postResponse.Data.Id.Should().BeGreaterThan(0);
        postResponse.Data.Name.Should().Be(newCategory.Name);
        postResponse.Data.Description.Should().Be(newCategory.Description);

        // Step 3: Send a PUT request to /api/v1/faqs/category with a valid FaqCategoryDto payload.
        var updatedCategory = new FaqCategoryDto
        {
            Id = postResponse.Data.Id,
            Name = "Updated Test Category",
            Description = "This is an updated test category"
        };

        var putResponse = await _apiClient.EditFaqCategoryAsync(updatedCategory);

        // Step 4: Verify the response status is 200 and the response body matches the FaqCategoryDto schema.
        putResponse.StatusCode.Should().Be(200);
        putResponse.IsSuccessStatusCode.Should().BeTrue();
        putResponse.Data.Should().NotBeNull();
        putResponse.Data.Id.Should().Be(updatedCategory.Id);
        putResponse.Data.Name.Should().Be(updatedCategory.Name);
        putResponse.Data.Description.Should().Be(updatedCategory.Description);
    }
}