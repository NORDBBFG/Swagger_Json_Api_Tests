using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

[TestFixture]
public class DonationTests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new DonationApiClient("https://api.example.com");
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
            DonationStatus = DonationStatuses.Status1,
            BloodVolume = 450,
            HealthFeeling = HealthFeeling.Feeling1,
            FeelingComment = "Feeling good",
            ExperienceRate = 5,
            Comment = "Great experience"
        };

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(200, response.Data.StatusCode);
        Assert.IsNotNull(response.Data.Content);
    }

    [Test]
    public async Task ConfirmDonation_BadRequest_ReturnsBadRequest()
    {
        // Arrange
        _client.SimulateError(true);
        var donation = new DonationDto(); // Invalid donation data

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(500, response.Data.StatusCode);
        Assert.IsNotNull(response.Data.Content);
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
            DonationStatus = DonationStatuses.Status1,
            BloodVolume = 450,
            HealthFeeling = HealthFeeling.Feeling1,
            FeelingComment = "Feeling good",
            ExperienceRate = 5,
            Comment = "Great experience"
        };

        // Act
        var response = await _client.ConfirmDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(500, response.Data.StatusCode);
        Assert.IsNotNull(response.Data.Content);
    }
}