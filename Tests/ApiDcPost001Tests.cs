// ApiDcPost001Tests.cs
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DonorApp.ApiClient;
using DonorApp.Services.DTO;
using NUnit.Framework;
using FluentAssertions;

namespace DonorApp.Tests.Api
{
    [TestFixture]
    public class ApiDcPost001Tests
    {
        private DonationCenterApiClient _apiClient;

        [SetUp]
        public void Setup()
        {
            var httpClient = new HttpClient();
            _apiClient = new DonationCenterApiClient(httpClient);
        }

        [Test]
        public async Task AddDonationCenter_ValidPayload_ReturnsCreatedDonationCenter()
        {
            // Arrange
            var newDonationCenter = new DonationCenterDto
            {
                Name = "Test Donation Center",
                Region = "Test Region",
                City = "Test City",
                Address = "123 Test Street",
                Phones = new List<string> { "+1234567890" },
                Comment = "Test comment",
                Latitude = 40.7128,
                Longitude = -74.0060,
                VisibleDays = true,
                DutySchedule = new List<string> { "Monday", "Wednesday", "Friday" },
                LinkOnTelegram = "https://t.me/testcenter"
            };

            // Act
            var response = await _apiClient.AddDonationCenterAsync(newDonationCenter);

            // Assert
            response.IsSuccess.Should().BeTrue();
            response.StatusCode.Should().Be(200);
            response.Data.Should().NotBeNull();
            response.Data.Should().BeEquivalentTo(newDonationCenter, options => options
                .Excluding(dc => dc.Id)
                .Excluding(dc => dc.CreatedDate)
                .Excluding(dc => dc.UpdatedDate)
                .Excluding(dc => dc.CreatedBy)
                .Excluding(dc => dc.LastUpdatedBy)
                .Excluding(dc => dc.CreatedByName)
                .Excluding(dc => dc.LastUpdatedByName)
            );
            response.Data.Id.Should().BeGreaterThan(0);
            response.Data.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            response.Data.UpdatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        }
    }
}