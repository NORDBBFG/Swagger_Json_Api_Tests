using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using YourNamespace.ApiClients;
using YourNamespace.Models;
using FluentAssertions;

namespace YourNamespace.Tests
{
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
        public async Task AddFaqCategory_ShouldReturnAddedCategory()
        {
            // Arrange
            var newCategory = new FaqCategoryDto
            {
                Name = "Test Category",
                Description = "This is a test category"
            };

            // Act
            var response = await _apiClient.AddFaqCategoryAsync(newCategory);

            // Assert
            response.StatusCode.Should().Be(200);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.Id.Should().BeGreaterThan(0);
            response.Data.Name.Should().Be(newCategory.Name);
            response.Data.Description.Should().Be(newCategory.Description);
            response.Data.Created.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            response.Data.Updated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        }

        [Test]
        public async Task EditFaqCategory_ShouldReturnEditedCategory()
        {
            // Arrange
            var existingCategory = new FaqCategoryDto
            {
                Id = 1, // Assume this category exists
                Name = "Existing Category",
                Description = "This is an existing category"
            };

            var updatedCategory = new FaqCategoryDto
            {
                Id = existingCategory.Id,
                Name = "Updated Category Name",
                Description = "This is an updated category description"
            };

            // Act
            var response = await _apiClient.EditFaqCategoryAsync(updatedCategory);

            // Assert
            response.StatusCode.Should().Be(200);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.Id.Should().Be(existingCategory.Id);
            response.Data.Name.Should().Be(updatedCategory.Name);
            response.Data.Description.Should().Be(updatedCategory.Description);
            response.Data.Updated.Should().BeAfter(response.Data.Created);
        }
    }
}