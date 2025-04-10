// DonationTests.cs
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
        _client = new DonationApiClient("http://api.example.com");
    }

    [Test]
    public async Task AddDonation_SuccessfulRequest_ReturnsOk()
    {
        // Arrange
        var donation = CreateSampleDonation();

        // Act
        var response = await _client.AddDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(200, response.Data.StatusCode);
    }

    [Test]
    public async Task AddDonation_BadRequest_ReturnsBadRequest()
    {
        // Arrange
        var donation = new DonationDto(); // Invalid donation

        // Act
        var response = await _client.AddDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(400, response.Data.StatusCode);
    }

    [Test]
    public async Task AddDonation_SimulatedError_ReturnsInternalServerError()
    {
        // Arrange
        var donation = CreateSampleDonation();
        _client.SimulateError(true);

        // Act
        var response = await _client.AddDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(500, response.Data.StatusCode);
    }

    [Test]
    public async Task EditDonation_SuccessfulRequest_ReturnsOk()
    {
        // Arrange
        var donation = CreateSampleDonation();

        // Act
        var response = await _client.EditDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(200, response.Data.StatusCode);
    }

    [Test]
    public async Task EditDonation_BadRequest_ReturnsBadRequest()
    {
        // Arrange
        var donation = new DonationDto(); // Invalid donation

        // Act
        var response = await _client.EditDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(400, response.Data.StatusCode);
    }

    [Test]
    public async Task EditDonation_SimulatedError_ReturnsInternalServerError()
    {
        // Arrange
        var donation = CreateSampleDonation();
        _client.SimulateError(true);

        // Act
        var response = await _client.EditDonationAsync(donation);

        // Assert
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNotNull(response.Data);
        Assert.AreEqual(500, response.Data.StatusCode);
    }

    private DonationDto CreateSampleDonation()
    {
        return new DonationDto
        {
            Id = Guid.NewGuid(),
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            Date = DateTime.Now,
            BloodType = "A+",
            DonationTypeId = 1,
            CityId = 1,
            DonationCenterId = 1,
            DonationStatus = DonationStatuses.Status1,
            BloodVolume = 450,
            HealthFeeling = HealthFeeling.Feeling3,
            FeelingComment = "Feeling good",
            ExperienceRate = 4,
            Comment = "Great experience"
        };
    }
}