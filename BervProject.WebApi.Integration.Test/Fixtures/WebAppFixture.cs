namespace BervProject.WebApi.Integration.Test.Fixtures;

using Microsoft.AspNetCore.Mvc.Testing;

public class WebAppFixture : IDisposable
{
    public WebAppFixture()
    {
        WebApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(_ =>
            {
                // ... Configure test services
            });

    }

    public void Dispose() => WebApp.Dispose();

    public WebApplicationFactory<Program> WebApp { get; }
}