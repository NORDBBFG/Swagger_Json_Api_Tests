using System;
using NUnit.Framework;
using FluentAssertions;
using System.Threading.Tasks;

[TestFixture]
public class ApiTc001Tests
{
    private DonationTypeApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        string baseUrl = "https://api.example.com"; // Replace with actual base URL
        string token = "valid_token"; // Replace with actual token
        _apiClient = new DonationTypeApiClient(baseUrl, token);
    }

    [Test]
    public async Task UpdateDonationType_ValidPayload_ReturnsSuccessResponse()
    {
        // Arrange
        var donationTypeDto = new DonationTypeDto
        {
            Id = 1,
            Name = "Updated Donation Type",
            ShortDescription = "Updated short description",
            LongDescription = "Updated long description",
            IconData = "base64encodedstring",
            CreateDate = DateTime.Parse("2023-01-01T12:00:00Z"),
            CreatedBy = "admin",
            UpdateDate = DateTime.Parse("2023-10-01T12:00:00Z"),
            LastUpdatedBy = "editor",
            HeaderColor = "#FFFFFF"
        };

        // Act
        var response = await _apiClient.UpdateDonationTypeAsync(donationTypeDto);

        // Assert
        response.StatusCode.Should().Be(200);
        response.Data.Should().NotBeNull();
        response.Data.Content.Should().Be("Donation type successfully updated.");
        response.Data.ContentType.Should().Be("application/json");
        response.Data.StatusCode.Should().Be(200);
    }
}