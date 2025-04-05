using BervProject.WebApi.Boilerplate.EntityFramework;
using BervProject.WebApi.Boilerplate.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
builder.AddNpgsqlDbContext<BoilerplateDbContext>("BoilerplateConnectionString");

var host = builder.Build();
host.Run();
