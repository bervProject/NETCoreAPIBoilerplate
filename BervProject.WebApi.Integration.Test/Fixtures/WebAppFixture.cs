namespace BervProject.WebApi.Integration.Test.Fixtures;

using Aspire.Hosting.Testing;
using Projects;

public class WebAppFixture : IAsyncLifetime
{
    // CI environments are slower — use a longer timeout when running in CI
    private static readonly TimeSpan DefaultTimeout =
        Environment.GetEnvironmentVariable("CI") is not null
            ? TimeSpan.FromMinutes(10)
            : TimeSpan.FromMinutes(3);

    private DistributedApplication? _app;

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<BervProject_WebApi_Boilerplate_AppHost>();

        _app = await appHost.BuildAsync()
            .WaitAsync(DefaultTimeout);

        await _app.StartAsync()
            .WaitAsync(DefaultTimeout);

        using var cts = new CancellationTokenSource(DefaultTimeout);

        // migrations has WithExplicitStart — trigger it via ResourceCommands then wait for
        // it to finish. apiservice has .WaitForCompletion(migration), so it won't start until
        // migrations is Finished.
        await _app.ResourceCommands.ExecuteCommandAsync("migrations", "start", cts.Token);
        await _app.ResourceNotifications
            .WaitForResourceAsync("migrations", KnownResourceStates.Finished, cts.Token);
        await _app.ResourceNotifications
            .WaitForResourceHealthyAsync("apiservice", cts.Token);
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
            await _app.DisposeAsync();
    }

    public HttpClient CreateApiClient()
        => _app!.CreateHttpClient("apiservice");
}
