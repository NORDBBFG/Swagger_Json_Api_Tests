using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RestApiNUnitTests
{
    [TestFixture]
    public class DonationTypeTests
    {
        private RestClient client;
        private const string BaseUrl = "https://api.example.com";

        [SetUp]
        public void Setup()
        {
            client = new RestClient(BaseUrl);
        }

        [Test]
        public async Task GetAllDonationTypes_ReturnsSuccessStatusCode()
        {
            var request = new RestRequest("/api/v1/donation/types", Method.GET);
            var response = await client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetAllDonationTypes_ReturnsListOfDonationTypes()
        {
            var request = new RestRequest("/api/v1/donation/types", Method.GET);
            var response = await client.ExecuteAsync(request);

            var donationTypes = JsonConvert.DeserializeObject<List<DonationTypeDto>>(response.Content);
            Assert.That(donationTypes, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task AddDonationType_ReturnsSuccessStatusCode()
        {
            var newDonationType = new DonationTypeDto
            {
                Name = "Test Donation Type",
                ShortDescription = "Short description",
                LongDescription = "Long description"
            };

            var request = new RestRequest("/api/v1/donation/types", Method.POST)
                .AddJsonBody(newDonationType);

            var response = await client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task EditDonationType_ReturnsSuccessStatusCode()
        {
            var updatedDonationType = new DonationTypeDto
            {
                Id = 1, // Assuming this ID exists
                Name = "Updated Donation Type",
                ShortDescription = "Updated short description",
                LongDescription = "Updated long description"
            };

            var request = new RestRequest("/api/v1/donation/types", Method.PUT)
                .AddJsonBody(updatedDonationType);

            var response = await client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task DeleteDonationType_ReturnsSuccessStatusCode()
        {
            int donationTypeIdToDelete = 1; // Assuming this ID exists

            var request = new RestRequest($"/api/v1/donation/types/{donationTypeIdToDelete}", Method.DELETE);

            var response = await client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }

    public class DonationTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string IconData { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string HeaderColor { get; set; }
    }
}