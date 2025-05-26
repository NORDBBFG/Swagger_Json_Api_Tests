using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

[TestFixture]
public class ApiDnGet001Tests
{
    private DonationApiClient _client;

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        _client = new DonationApiClient(httpClient);
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
            Page = new PageFilter { PageNumber = 1, PageSize = 20 }
        };

        // Act
        var response = await _client.GetDonationsXlsxAsync(filter);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType.MediaType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }

    [Test]
    public async Task GetDonationsXlsx_InvalidFilter_ReturnsBadRequest()
    {
        // Arrange
        var filter = new DonationFilter
        {
            // Invalid filter with missing required fields
            FullName = "John Doe"
        };

        // Act
        var response = await _client.GetDonationsXlsxAsync(filter);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await response.Content.ReadAsStringAsync();
        var errorResult = JsonConvert.DeserializeObject<ContentResult>(content);
        errorResult.Should().NotBeNull();
        errorResult.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task GetDonationsXlsx_ServerError_ReturnsInternalServerError()
    {
        // Arrange
        var filter = new DonationFilter
        {
            // Valid filter that might trigger a server error
            FullName = "Error Trigger",
            DateFrom = DateTime.Now.AddYears(-100),
            DateTo = DateTime.Now
        };

        // Act
        var response = await _client.GetDonationsXlsxAsync(filter);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await response.Content.ReadAsStringAsync();
        var errorResult = JsonConvert.DeserializeObject<ContentResult>(content);
        errorResult.Should().NotBeNull();
        errorResult.StatusCode.Should().Be(500);
    }
}
