using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

[TestFixture]
public class DonationTests
{
    private DonationApiClient _client;
    private const string BaseUrl = "https://api.example.com";

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task ConfirmDonation_SuccessfulRequest_ReturnsOk()
    {
        // Arrange
        var donation = new DonationDto
        {
            Id = Guid.NewGuid(),
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            Date = DateTime.UtcNow,
            BloodType = "A+",
            DonationTypeId = 1,
            CityId = 1,
            DonationCenterId = 1,
            BloodVolume = 450,
            ExperienceRate = 5
        };

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(response.Content);
        Assert.AreEqual(200, response.Content.StatusCode);
    }

    [Test]
    public async Task ConfirmDonation_BadRequest_ReturnsBadRequest()
    {
        // Arrange
        var invalidDonation = new DonationDto(); // Invalid donation with missing required fields

        // Act
        var response = await _client.ConfirmDonationAsync(invalidDonation);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response.Content);
        Assert.AreEqual(400, response.Content.StatusCode);
    }

    [Test]
    public async Task ConfirmDonation_ServerError_ReturnsInternalServerError()
    {
        // Arrange
        _client.SimulateError(true);
        var donation = new DonationDto
        {
            Id = Guid.NewGuid(),
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            Date = DateTime.UtcNow,
            BloodType = "A+",
            DonationTypeId = 1,
            CityId = 1,
            DonationCenterId = 1,
            BloodVolume = 450,
            ExperienceRate = 5
        };

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNotNull(response.Content);
        Assert.AreEqual(500, response.Content.StatusCode);
    }
}
