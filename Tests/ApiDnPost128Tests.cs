using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiDnPost128Tests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationApiClient(httpClient);
    }

    [Test]
    public async Task AddDonation_ValidData_ShouldReturnSuccessResponse()
    {
        // Arrange
        var donation = new DonationDto
        {
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            BloodType = "A+",
            DonationDate = DateTime.Now,
            DonationType = "Whole Blood",
            Region = "North",
            City = "Example City",
            DonationCenter = "City Hospital",
            Status = 1,
            BloodVolume = 450
        };

        // Act
        var response = await _client.AddDonationAsync(donation);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Donation successfully added");
        response.Data.StatusCode.Should().Be(200);
    }
}

[TestFixture]
public class ApiDnPost129Tests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationApiClient(httpClient);
    }

    [Test]
    public async Task AddDonation_InvalidData_ShouldReturnBadRequestResponse()
    {
        // Arrange
        var invalidDonation = new DonationDto
        {
            // Missing required fields
            UserId = "",
            FirstName = "",
            LastName = "",
            BloodType = "Invalid",
            DonationDate = DateTime.Now,
            DonationType = "",
            Region = "",
            City = "",
            DonationCenter = "",
            Status = -1,
            BloodVolume = -100
        };

        // Act
        var response = await _client.AddDonationAsync(invalidDonation);

        // Assert
        response.StatusCode.Should().Be(400);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Contain("Bad request");
        response.Data.StatusCode.Should().Be(400);
    }
}
