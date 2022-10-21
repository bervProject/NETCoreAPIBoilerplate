using BervProject.WebApi.Integration.Test.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BervProject.WebApi.Integration.Test
{
    [Collection("Webapp")]
    public class HealthCheckTest
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        public HealthCheckTest(WebAppFixture webAppFixtures)
        {
            this._applicationFactory = webAppFixtures.WebApp;
        }
        [Fact]
        public async Task SuccessCheck()
        {
            var client = _applicationFactory.CreateClient();
            var response = await client.GetAsync("/healthz");
            Assert.True(response.IsSuccessStatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Equal("Healthy", stringResponse);
        }
    }
}