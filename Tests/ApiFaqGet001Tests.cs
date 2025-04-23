using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using YourNamespace.ApiClient;
using YourNamespace.Models;

namespace YourNamespace.Tests
{
    [TestFixture]
    public class ApiFaqGet001Tests
    {
        private HttpClient _httpClient;
        private FaqApiClient _faqApiClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://your-api-base-url.com/")
            };
            _faqApiClient = new FaqApiClient(_httpClient);
        }

        [Test]
        public async Task GetFaqArticles_ReturnsSuccessfulResponse()
        {
            // Act
            var response = await _faqApiClient.GetFaqArticlesAsync();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Data.Should().NotBeNull().And.BeAssignableTo<List<FaqArticleDto>>();
            response.Data.Should().NotBeEmpty();

            foreach (var faqArticle in response.Data)
            {
                faqArticle.Id.Should().BeGreaterThan(0);
                faqArticle.Question.Should().BeOfType<string>().Or.BeNull();
                faqArticle.Answer.Should().BeOfType<string>().Or.BeNull();
                faqArticle.Picture.Should().BeOfType<string>().Or.BeNull();
                faqArticle.IsDraft.Should().BeOfType<bool>();
                faqArticle.IsPermanent.Should().BeOfType<bool>();
                faqArticle.Created.Should().BeAfter(DateTime.MinValue);
                faqArticle.Updated.Should().BeAfter(DateTime.MinValue);

                faqArticle.Category.Should().NotBeNull();
                faqArticle.Category.Id.Should().BeGreaterThan(0);
                faqArticle.Category.Name.Should().BeOfType<string>().Or.BeNull();

                faqArticle.RelatedQuestions.Should().BeOfType<List<RelatedFaqArticleDto>>().Or.BeNull();
                if (faqArticle.RelatedQuestions != null)
                {
                    foreach (var relatedQuestion in faqArticle.RelatedQuestions)
                    {
                        relatedQuestion.Id.Should().BeGreaterThan(0);
                        relatedQuestion.Question.Should().BeOfType<string>().Or.BeNull();
                    }
                }

                faqArticle.CreatedBy.Should().BeOfType<string>().Or.BeNull();
                faqArticle.LastUpdatedBy.Should().BeOfType<string>().Or.BeNull();
                faqArticle.CreatedByName.Should().BeOfType<string>().Or.BeNull();
                faqArticle.LastUpdatedByName.Should().BeOfType<string>().Or.BeNull();
            }
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}