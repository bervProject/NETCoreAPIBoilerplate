namespace BervProject.WebApi.Integration.Test;

using Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

[Collection("Webapp")]
public class ErrorControllerTest
{
    private readonly WebApplicationFactory<Program> _applicationFactory;
    public ErrorControllerTest(WebAppFixture webAppFixtures)
    {
        _applicationFactory = webAppFixtures.WebApp;
    }
    [Fact]
    public async Task SuccessCheck()
    {
        var client = _applicationFactory.CreateClient();
        var response = await client.GetAsync("/api/v1.0/error/error");
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(responseString);
        var status = jObject.Value<int>("status");
        var title = jObject.Value<string>("title");
        Assert.Equal("An error occurred while processing your request.", title);
        Assert.Equal(500, status);
    }
}