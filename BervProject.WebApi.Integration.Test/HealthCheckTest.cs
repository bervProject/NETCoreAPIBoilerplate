
using Microsoft.AspNetCore.Mvc.Testing;

namespace BervProject.WebApi.Integration.Test
{
    public class HealthCheckTest
    {
        [Fact]
        public async Task SuccessCheck()
        {
            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // ... Configure test services
                });

            var client = application.CreateClient();
            var response = await client.GetAsync("/healthz");
            Assert.True(response.IsSuccessStatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Equal("Healthy", stringResponse);
        }
    }
}