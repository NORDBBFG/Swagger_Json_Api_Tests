using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;

[TestFixture]
public class ApiDnGet001Tests
{
    private DonationCenterApiClient _client;
    private const string BaseUrl = "https://api.example.com"; // Replace with actual base URL

    [SetUp]
    public void Setup()
    {
        var httpClient = new HttpClient();
        // Add any necessary headers, like authorization
        // httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer <your_token>");
        _client = new DonationCenterApiClient(httpClient, BaseUrl);
    }

    [Test]
    public async Task GetDonationCenters_ReturnsSuccessfulResponse()
    {
        // Arrange
        string donationCenterName = "Sample Center";
        int regionId = 1;
        int cityId = 2;

        // Act
        var response = await _client.GetDonationCentersAsync(donationCenterName, regionId, cityId);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeAssignableTo<List<DonationCenterDto>>();

        if (response.Data != null && response.Data.Count > 0)
        {
            var firstCenter = response.Data[0];
            firstCenter.Should().NotBeNull();
            firstCenter.Id.Should().BeGreaterThan(0);
            firstCenter.Name.Should().NotBeNullOrEmpty();
            firstCenter.Region.Should().NotBeNullOrEmpty();
            firstCenter.City.Should().NotBeNullOrEmpty();
            firstCenter.CreatedDate.Should().BeBefore(DateTime.UtcNow);
            firstCenter.UpdatedDate.Should().BeBefore(DateTime.UtcNow);
        }
    }

    [Test]
    public async Task GetDonationCenters_WithInvalidParameters_ReturnsBadRequest()
    {
        // Arrange
        string invalidName = new string('A', 1000); // Assuming there's a max length for name

        // Act
        var response = await _client.GetDonationCentersAsync(invalidName);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        response.Data.Should().BeNull();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
