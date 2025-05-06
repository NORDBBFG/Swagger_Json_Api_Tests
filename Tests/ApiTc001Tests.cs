using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

[TestFixture]
public class ApiTc001Tests
{
    private HttpClient _httpClient;
    private DonationTypesApiClient _apiClient;

    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer <valid_token>");
        _apiClient = new DonationTypesApiClient(_httpClient);
    }

    [Test]
    public async Task GetDonationTypes_ReturnsSuccessfulResponse()
    {
        // Arrange
        // No additional arrangement needed as the client is set up in the Setup method

        // Act
        var response = await _apiClient.GetDonationTypesAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.IsSuccessStatusCode.Should().BeTrue();
        response.Data.Should().NotBeNull();
        response.Data.Should().BeOfType<List<DonationTypeDto>>();
        
        // Additional assertions can be added here to check the structure and content of the returned data
        // For example:
        // response.Data.Should().HaveCountGreaterThan(0);
        // response.Data[0].Should().BeOfType<DonationTypeDto>();
        // response.Data[0].Id.Should().BeGreaterThan(0);
        // response.Data[0].Name.Should().NotBeNullOrEmpty();
        // response.Data[0].Description.Should().NotBeNullOrEmpty();
    }

    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}