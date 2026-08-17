namespace BervProject.WebApi.Integration.Test;

using Boilerplate.Entities;
using Fixtures;
using System.Net.Http.Json;

[Collection("Webapp")]
public class WeatherForecastControllerTest
{
    private readonly WebAppFixture _fixture;

    public WeatherForecastControllerTest(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SuccessCheck()
    {
        var client = _fixture.CreateApiClient();
        var response = await client.GetAsync("/api/v1.0/weatherforecast/db");
        Assert.True(response.IsSuccessStatusCode);
        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.Equal(2, books.Count);
    }
}
