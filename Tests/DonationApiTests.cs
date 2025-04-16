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
    public async Task GetDonations_ValidFilter_ReturnsSuccessfulResponse()
    {
        var filter = new DonationFilter
        {
            FullName = "John Doe",
            BloodType = new List<string> { "A+", "O-" },
            DateFrom = new DateTime(2023, 1, 1),
            DateTo = new DateTime(2023, 12, 31, 23, 59, 59),
            DonationTypeIds = new List<int> { 1, 2 },
            RegionIds = new List<int> { 101 },
            CityIds = new List<int> { 202 },
            DonationCenterIds = new List<int> { 303 },
            Status = new List<int> { 1 },
            BloodVolume = new List<int> { 500 },
            Page = new PageFilter { PageNumber = 1, PageSize = 10 }
        };

        var response = await _client.GetDonationsAsync(filter);

        Assert.AreEqual(200, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.IsInstanceOf<List<DonationDto>>(response.Data);
        Assert.IsTrue(response.Data.Count > 0);

        foreach (var donation in response.Data)
        {
            Assert.IsNotNull(donation.Id);
            Assert.IsNotNull(donation.FirstName);
            Assert.IsNotNull(donation.LastName);
            Assert.IsNotNull(donation.BloodType);
            Assert.IsNotNull(donation.DonationType);
            Assert.IsNotNull(donation.Region);
            Assert.IsNotNull(donation.City);
            Assert.IsNotNull(donation.DonationCenter);
            Assert.Greater(donation.Status, 0);
            Assert.Greater(donation.BloodVolume, 0);
        }
    }

    [Test]
    public async Task GetDonations_InvalidFilter_ReturnsBadRequest()
    {
        var invalidFilter = new DonationFilter
        {
            DateFrom = DateTime.Now,
            DateTo = DateTime.Now.AddDays(-1)
        };

        var response = await _client.GetDonationsAsync(invalidFilter);

        Assert.AreEqual(400, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }

    [Test]
    public async Task GetDonations_SimulatedServerError_ReturnsInternalServerError()
    {
        _client.SimulateError(true);

        var filter = new DonationFilter();
        var response = await _client.GetDonationsAsync(filter);

        Assert.AreEqual(500, response.StatusCode);
        Assert.IsNull(response.Data);
        Assert.IsNotNull(response.ErrorMessage);
    }
}