using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiAt127Tests
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
            Id = Guid.NewGuid(),
            UserId = "user123",
            FirstName = "John",
            LastName = "Doe",
            BloodType = "A+",
            DonationDate = DateTime.UtcNow,
            DonationType = "Whole Blood",
            Region = "North",
            City = "Example City",
            DonationCenter = "Example Center",
            Status = 1,
            BloodVolume = 450
        };

        // Act
        var response = await _client.AddDonationAsync(donation);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddDonation_InvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidDonation = new DonationDto(); // Empty donation object

        // Act
        var response = await _client.AddDonationAsync(invalidDonation);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(400);
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task EditDonation_ValidData_ShouldReturnSuccessResponse()
    {
        // Arrange
        var donation = new DonationDto
        {
            Id = Guid.NewGuid(),
            UserId = "user123",
            FirstName = "Jane",
            LastName = "Doe",
            BloodType = "B-",
            DonationDate = DateTime.UtcNow,
            DonationType = "Plasma",
            Region = "South",
            City = "Another City",
            DonationCenter = "Another Center",
            Status = 2,
            BloodVolume = 400
        };

        // Act
        var response = await _client.EditDonationAsync(donation);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().NotBeNullOrEmpty();
        response.Data.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task EditDonation_InvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidDonation = new DonationDto { Id = Guid.Empty }; // Invalid ID

        // Act
        var response = await _client.EditDonationAsync(invalidDonation);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(400);
        response.ErrorContent.Should().NotBeNullOrEmpty();
    }
}
