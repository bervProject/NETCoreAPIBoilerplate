using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.SimpleEmail;
using Autofac.Extensions.DependencyInjection;
using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.EntityFramework;
using BervProject.WebApi.Boilerplate.Services;
using BervProject.WebApi.Boilerplate.Services.AWS;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
    logging.AddNLog("Nlog.config");
    logging.AddNLogWeb();
});
builder.Host.UseNLog();

// settings injection
var awsConfig = builder.Configuration.GetSection("AWS").Get<AWSConfiguration>();
builder.Services.AddSingleton(awsConfig);

var azureConfig = builder.Configuration.GetSection("Azure").Get<AzureConfiguration>();
builder.Services.AddSingleton(azureConfig);

// aws services
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddAWSService<IAmazonSimpleEmailService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDynamoDbServices, DynamoDbServices>();
builder.Services.AddScoped<IAWSS3Service, AWSS3Service>();

// azure services
builder.Services.AddAzureClients(builder =>
{
    builder.AddBlobServiceClient(azureConfig.Storage.Blob.ConnectionString);
    builder.AddQueueServiceClient(azureConfig.Storage.Queue.ConnectionString);
});
builder.Services.AddScoped<IAzureQueueServices, AzureQueueServices>();
builder.Services.AddScoped<ITopicServices, TopicServices>();
builder.Services.AddScoped<IAzureStorageQueueService, AzureStorageQueueService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddSingleton<IServiceBusQueueConsumer, ServiceBusQueueConsumer>();
builder.Services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
builder.Services.AddTransient<IProcessData, ProcessData>();
builder.Services.AddApplicationInsightsTelemetry();

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

app.UseSwagger(c =>
{
    c.RouteTemplate = "api/docs/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/docs/v1/swagger.json", "My API V1");
    c.RoutePrefix = "api/docs";
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});

app.Run();
