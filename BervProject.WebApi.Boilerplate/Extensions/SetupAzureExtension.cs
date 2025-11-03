namespace BervProject.WebApi.Boilerplate.Extensions;

using Entities;
using Services;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension for setup Azure services
/// </summary>
public static class SetupAzureExtension
{
    /// <summary>
    /// Setup Azure
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    public static void SetupAzure(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(config.GetConnectionString("AzureStorageBlob"));
            builder.AddQueueServiceClient(config.GetConnectionString("AzureStorageQueue"));
            builder.AddServiceBusClient(config.GetConnectionString("AzureServiceBus"));
            builder.AddTableServiceClient(config.GetConnectionString("AzureStorageTable"));
        });
        services.AddScoped<IAzureQueueServices, AzureQueueServices>();
        services.AddScoped<ITopicServices, TopicServices>();
        services.AddScoped<IAzureStorageQueueService, AzureStorageQueueService>();
        services.AddScoped<IBlobService, BlobService>();
        // add each tables
        services.AddScoped<IAzureTableStorageService<Note>, AzureTableStorageService<Note>>();
        // service bus
        services.AddSingleton<IServiceBusQueueConsumer, ServiceBusQueueConsumer>();
        services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
        services.AddTransient<IProcessData, ProcessData>();
        services.AddApplicationInsightsTelemetry();
    }
}
