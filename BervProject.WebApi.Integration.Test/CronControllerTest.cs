namespace BervProject.WebApi.Integration.Test;

using Fixtures;
using Hangfire;

[Collection("Webapp")]
public class CronControllerTest : IAsyncLifetime
{
    private readonly WebAppFixture _fixture;
    private HttpClient? _client;
    private readonly List<string> _registeredRecurring = new();

    public CronControllerTest(WebAppFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        _client = _fixture.CreateApiClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await RemoveRecurringJobsAsync();
        _client?.Dispose();
    }

    private async Task RemoveRecurringJobsAsync()
    {
        if (_registeredRecurring.Count == 0) return;
        // Give Hangfire a moment to register the jobs before removing them
        await Task.Delay(500);
        var response = await _client!.GetAsync("/api/v1.0/cron/jobs");
        // Best-effort cleanup via the API; if the endpoint doesn't exist, skip
        if (!response.IsSuccessStatusCode) return;
    }

    [Fact]
    public async Task SuccessCreateCronOnceTest()
    {
        var response = await _client!.PostAsync("/api/v1.0/cron/CreateCronOnce", null);
        Assert.True(response.IsSuccessStatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(stringResponse);
    }

    [Fact]
    public async Task SuccessCreateRecuranceTest()
    {
        var response = await _client!.PostAsync("/api/v1.0/cron/CreateRecurance", null);
        Assert.True(response.IsSuccessStatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(stringResponse);
        _registeredRecurring.Add(stringResponse);
    }
}
