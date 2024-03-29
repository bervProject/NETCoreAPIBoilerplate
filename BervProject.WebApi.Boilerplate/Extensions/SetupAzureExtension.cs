﻿namespace BervProject.WebApi.Boilerplate.Extenstions;

using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.Entities;
using BervProject.WebApi.Boilerplate.Services.Azure;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;


public static class SetupAzureExtension
{
    public static void SetupAzure(this IServiceCollection services, AzureConfiguration azureConfig)
    {
        services.AddAzureClients(builder =>
        {
            builder.AddBlobServiceClient(azureConfig.Storage.Blob.ConnectionString);
            builder.AddQueueServiceClient(azureConfig.Storage.Queue.ConnectionString);
            builder.AddServiceBusClient(azureConfig.ServiceBus.ConnectionString);
            builder.AddTableServiceClient(azureConfig.Storage.Table.ConnectionString);
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
