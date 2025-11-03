namespace BervProject.WebApi.Integration.Test;

using Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

[Collection("Webapp")]
public class HealthCheckTest
{
    private readonly WebApplicationFactory<Program> _applicationFactory;
    public HealthCheckTest(WebAppFixture webAppFixtures)
    {
        _applicationFactory = webAppFixtures.WebApp;
    }
    [Fact]
    public async Task SuccessCheck()
    {
        var client = _applicationFactory.CreateClient();
        var response = await client.GetAsync("/health");
        Assert.True(response.IsSuccessStatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", stringResponse);
    }
}