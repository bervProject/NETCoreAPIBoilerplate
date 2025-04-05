using Microsoft.Extensions.Configuration;

namespace BervProject.WebApi.Boilerplate.Extenstions;

using Entities;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;


public static class SetupAzureExtension
{
    public static void SetupAzure(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(config.GetConnectionString("Azure__Storage__Blob"));
            builder.AddQueueServiceClient(config.GetConnectionString("Azure__Storage__Queue"));
            builder.AddServiceBusClient(config.GetConnectionString("Azure__ServiceBus"));
            builder.AddTableServiceClient(config.GetConnectionString("Azure__Storage__Table"));
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
