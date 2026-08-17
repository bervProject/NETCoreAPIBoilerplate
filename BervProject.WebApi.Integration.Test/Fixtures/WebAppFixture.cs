namespace BervProject.WebApi.Integration.Test.Fixtures;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Projects;

public class WebAppFixture : IAsyncLifetime
{
    private DistributedApplication? _app;

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<BervProject_WebApi_Boilerplate_AppHost>();

        _app = await appHost.BuildAsync();
        await _app.StartAsync();

        var rns = _app.Services.GetRequiredService<ResourceNotificationService>();
        var rcs = _app.Services.GetRequiredService<ResourceCommandService>();

        // migrations uses WithExplicitStart — trigger the "start" command manually
        await rcs.ExecuteCommandAsync("migrations", "start");
        await rns.WaitForResourceAsync("migrations", KnownResourceStates.Finished);
        await rns.WaitForResourceHealthyAsync("apiservice");
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
            await _app.DisposeAsync();
    }

    public HttpClient CreateApiClient()
        => _app!.CreateHttpClient("apiservice");
}
