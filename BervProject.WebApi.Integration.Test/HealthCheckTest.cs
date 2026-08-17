namespace BervProject.WebApi.Integration.Test;

using Fixtures;

[Collection("Webapp")]
public class HealthCheckTest
{
    private readonly WebAppFixture _fixture;

    public HealthCheckTest(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SuccessCheck()
    {
        var client = _fixture.CreateApiClient();
        var response = await client.GetAsync("/health");
        Assert.True(response.IsSuccessStatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", stringResponse);
    }
}
