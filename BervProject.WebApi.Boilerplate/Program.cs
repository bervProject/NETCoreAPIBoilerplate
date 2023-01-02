using Amazon.S3.Model;
using Autofac.Extensions.DependencyInjection;
using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.EntityFramework;
using BervProject.WebApi.Boilerplate.Extenstions;
using BervProject.WebApi.Boilerplate.Services;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Logging.AddNLog("Nlog.config");
builder.Logging.AddNLogWeb();
builder.Host.UseNLog();

// settings injection
var awsConfig = builder.Configuration.GetSection("AWS").Get<AWSConfiguration>();
builder.Services.AddSingleton(awsConfig);

var azureConfig = builder.Configuration.GetSection("Azure").Get<AzureConfiguration>();
builder.Services.AddSingleton(azureConfig);

// aws services
builder.Services.SetupAWS();

// azure services
builder.Services.SetupAzure(azureConfig);

// cron services
builder.Services.AddScoped<ICronService, CronService>();
builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("BoilerplateConnectionString")));
builder.Services.AddHangfireServer();

// essential services
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});
builder.Services.AddDbContext<BoilerplateDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("BoilerplateConnectionString")));

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddApiVersioning();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// register Consumer
var connectionString = azureConfig.ServiceBus.ConnectionString;
var queueName = azureConfig.ServiceBus.QueueName;
var topicName = azureConfig.ServiceBus.TopicName;
if (!string.IsNullOrWhiteSpace(queueName) && !string.IsNullOrWhiteSpace(connectionString))
{
    var bus = app.Services.GetService<IServiceBusQueueConsumer>();
    bus.RegisterOnMessageHandlerAndReceiveMessages();
}
if (!string.IsNullOrWhiteSpace(topicName) && !string.IsNullOrWhiteSpace(connectionString))
{
    var bus = app.Services.GetService<IServiceBusTopicSubscription>();
    bus.RegisterOnMessageHandlerAndReceiveMessages();
}

// register essential things
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseAuthorization();

app.MapHealthChecks("/healthz");

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/docs/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/docs/v1/swagger.json", "My API V1");
    c.RoutePrefix = "api/docs";
});

app.MapControllers();
app.MapHangfireDashboard();

app.Run();

public partial class Program { }
