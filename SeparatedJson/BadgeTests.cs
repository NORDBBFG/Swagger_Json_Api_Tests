using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestApiNUnitTests
{
    [TestFixture]
    public class BadgeTests
    {
        private BadgeApiClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new BadgeApiClient("https://api.example.com");
        }

        [Test]
        public async Task GetBadges_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetBadgesAsync();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task GetBadges_ReturnsListOfBadges()
        {
            var badges = await _client.GetBadgesAsync();
            Assert.IsInstanceOf<List<BadgeDto>>(badges);
            Assert.IsNotEmpty(badges);
        }

        [Test]
        public async Task GetBadges_BadRequest_ReturnsErrorMessage()
        {
            // Simulate a bad request
            _client.SetInvalidParameter();
            var response = await _client.GetBadgesAsync();
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.IsNotNull(response.ErrorMessage);
        }

        [Test]
        public async Task GetBadges_InternalServerError_ReturnsErrorMessage()
        {
            // Simulate an internal server error
            _client.SimulateServerError();
            var response = await _client.GetBadgesAsync();
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsNotNull(response.ErrorMessage);
        }
    }
}
