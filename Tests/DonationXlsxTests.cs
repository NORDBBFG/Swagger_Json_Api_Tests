using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class DonationXlsxTests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationApiClient("https://api.example.com"); // Replace with actual base URL
    }

    [Test]
    public async Task GetDonationsXlsx_ValidFilter_ReturnsXlsxFile()
    {
        // Arrange
        var filter = new DonationFilter
        {
            FullName = "John Doe",
            BloodType = new List<string> { "A+", "B-" },
            DateFrom = DateTime.Now.AddDays(-30),
            DateTo = DateTime.Now,
            DonationTypeIds = new List<int> { 1, 2 },
            RegionIds = new List<int> { 10, 20 },
            CityIds = new List<int> { 100, 200 },
            DonationCenterIds = new List<int> { 1000, 2000 },
            Status = new List<int> { 1, 2 },
            BloodVolume = new List<int> { 450, 500 },
            Page = new PageFilter { Number = 1, PackageSize = 50 }
        };

        // Act
        var response = await _client.GetDonationsXlsxAsync(filter);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Length.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task GetDonationsXlsx_InvalidFilter_ReturnsBadRequest()
    {
        // Arrange
        var filter = new DonationFilter
        {
            // Invalid filter: missing required fields
            FullName = "John Doe"
        };

        // Act
        var response = await _client.GetDonationsXlsxAsync(filter);

        // Assert
        response.StatusCode.Should().Be(400);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(400);
        response.ErrorResult.Content.Should().Contain("Bad request");
    }

    [Test]
    public async Task GetDonationsXlsx_ServerError_ReturnsInternalServerError()
    {
        // Arrange
        var filter = new DonationFilter
        {
            // Valid filter that might trigger a server error
            FullName = "Error Trigger",
            DateFrom = DateTime.Now.AddDays(-30),
            DateTo = DateTime.Now,
            Page = new PageFilter { Number = 1, PackageSize = 50 }
        };

        // Act
        var response = await _client.GetDonationsXlsxAsync(filter);

        // Assert
        response.StatusCode.Should().Be(500);
        response.ErrorResult.Should().NotBeNull();
        response.ErrorResult.StatusCode.Should().Be(500);
        response.ErrorResult.Content.Should().Contain("Internal server error");
    }
}