using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using YourNamespace.ApiClient;
using YourNamespace.Models;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class FAQ001Tests
    {
        private FaqApiClient _faqApiClient;

        [SetUp]
        public void Setup()
        {
            _faqApiClient = new FaqApiClient("https://your-api-base-url.com");
        }

        [Test]
        public async Task GetFaqsByFilter_ValidCriteria_ReturnsMatchingFaqs()
        {
            // Arrange
            var filter = new FaqFilter
            {
                CategoryId = 1,
                IsDraft = false,
                CreatedBy = "admin",
                Search = "donation"
            };

            // Act
            var response = await _faqApiClient.GetFaqsByFilterAsync(filter);

            // Assert
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeOfType<List<FaqArticleDto>>();

            foreach (var faq in response.Data)
            {
                faq.Category.Id.Should().Be(filter.CategoryId);
                faq.IsDraft.Should().Be(false);
                faq.CreatedBy.Should().Be(filter.CreatedBy);
                (faq.Question.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) ||
                 faq.Answer.Contains(filter.Search, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();

                // Verify required fields
                faq.Id.Should().BeGreaterThan(0);
                faq.Question.Should().NotBeNullOrEmpty();
                faq.Answer.Should().NotBeNullOrEmpty();
                faq.IsDraft.Should().BeFalse();
                faq.IsPermanent.Should().BeOneOf(true, false);
                faq.Created.Should().BeBefore(DateTime.UtcNow);
                faq.Updated.Should().BeBefore(DateTime.UtcNow);
                faq.Category.Should().NotBeNull();
            }
        }
    }
}