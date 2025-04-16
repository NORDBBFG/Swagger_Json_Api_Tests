using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class DonationApiTests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationApiClient("https://api.example.com");
    }

    [Test]
    public async Task GetDonations_SuccessfulRequest_ReturnsOkWithDonations()
    {
        // Arrange
        var filter = new DonationFilter
        {
            FullName = "John Doe",
            BloodType = new List<string> { "A+" },
            DateFrom = DateTime.Now.AddDays(-30),
            DateTo = DateTime.Now,
            Page = new PageFilter { PageNumber = 1, PageSize = 10 }
        };

        // Act
        var response = await _client.GetDonationsAsync(filter);

        // Assert
        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<DonationDto>>(response.Data);
        Assert.Greater(response.Data.Count, 0);
    }

    [Test]
    public async Task GetDonations_BadRequest_ReturnsBadRequestError()
    {
        // Arrange
        var filter = new DonationFilter
        {
            DateFrom = DateTime.Now,
            DateTo = DateTime.Now.AddDays(-1) // Invalid date range
        };

        // Act
        var response = await _client.GetDonationsAsync(filter);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotEmpty(response.ErrorMessage);
    }

    [Test]
    public async Task GetDonations_InternalServerError_ReturnsInternalServerError()
    {
        // Arrange
        _client.SimulateError(true);
        var filter = new DonationFilter();

        // Act
        var response = await _client.GetDonationsAsync(filter);

        // Assert
        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotEmpty(response.ErrorMessage);
    }
}