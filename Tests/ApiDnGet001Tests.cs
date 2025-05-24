using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.ApiClient;
using DonorApp.Services.DTO;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using Moq.Protected;

namespace DonorApp.Tests
{
    [TestFixture]
    public class ApiDnGet001Tests
    {
        private DonationTypesApiClient _client;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _client = new DonationTypesApiClient(httpClient);
        }

        [Test]
        public async Task AT_144_ValidateSuccessfulRetrievalOfAllDonationTypes()
        {
            // Arrange
            var expectedDonationTypes = new List<DonationTypeDto>
            {
                new DonationTypeDto { Id = Guid.NewGuid(), Name = "Whole Blood", Description = "Standard blood donation" },
                new DonationTypeDto { Id = Guid.NewGuid(), Name = "Plasma", Description = "Plasma only donation" }
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedDonationTypes))
                });

            // Act
            var response = await _client.GetDonationTypesAsync();

            // Assert
            response.StatusCode.Should().Be(200);
            response.IsSuccessStatusCode.Should().BeTrue();
            response.Data.Should().NotBeNull();
            response.Data.Should().HaveCount(2);
            response.Data.Should().BeEquivalentTo(expectedDonationTypes);
        }

        [Test]
        public async Task AT_145_ValidateErrorHandlingWhenAccessingWithServerIssues()
        {
            // Arrange
            var expectedError = new ContentResult
            {
                Content = "Internal Server Error",
                StatusCode = 500
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(expectedError))
                });

            // Act
            var response = await _client.GetDonationTypesAsync();

            // Assert
            response.StatusCode.Should().Be(500);
            response.IsSuccessStatusCode.Should().BeFalse();
            response.Data.Should().BeNull();
            response.Error.Should().NotBeNull();
            response.Error.Should().BeEquivalentTo(expectedError);
        }
    }
}