
using BervProject.WebApi.Boilerplate.Entities;
using BervProject.WebApi.Boilerplate.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace BervProject.WebApi.Integration.Test
{
    public class WeatherForecastControllerTest
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
            var response = await client.GetAsync("/api/v1.0/weatherforecast/db");
            Assert.True(response.IsSuccessStatusCode);
            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            Assert.NotNull(books);
            Assert.Equal(2, books.Count);
        }
    }
}