
using BervProject.WebApi.Boilerplate.Entities;
using BervProject.WebApi.Integration.Test.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace BervProject.WebApi.Integration.Test
{
    [Collection("Webapp")]
    public class WeatherForecastControllerTest
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        public WeatherForecastControllerTest(WebAppFixture webAppFixtures)
        {
            this._applicationFactory = webAppFixtures.WebApp;
        }
        [Fact]
        public async Task SuccessCheck()
        {
            var client = _applicationFactory.CreateClient();
            var response = await client.GetAsync("/api/v1.0/weatherforecast/db");
            Assert.True(response.IsSuccessStatusCode);
            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            Assert.NotNull(books);
            Assert.Equal(2, books.Count);
        }
    }
}