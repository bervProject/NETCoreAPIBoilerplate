using Amazon.DynamoDBv2;
using Amazon.S3;
using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.EntityFramework;
using BervProject.WebApi.Boilerplate.Services;
using BervProject.WebApi.Boilerplate.Services.AWS;
using BervProject.WebApi.Boilerplate.Services.Azure;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BervProject.WebApi.Boilerplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var awsConfig = Configuration.GetSection("AWS").Get<AWSConfiguration>();
            services.AddSingleton(awsConfig);

            var azureConfig = Configuration.GetSection("Azure").Get<AzureConfiguration>();
            services.AddSingleton(azureConfig);

            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(azureConfig.Storage.Blob.ConnectionString);
            });

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDynamoDbServices, DynamoDbServices>();
            services.AddScoped<IAzureQueueServices, AzureQueueServices>();
            services.AddScoped<ITopicServices, TopicServices>();
            services.AddScoped<IAzureStorageQueueService, AzureStorageQueueService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<ICronService, CronService>();
            services.AddScoped<IAWSS3Service, AWSS3Service>();

            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddSingleton<IServiceBusQueueConsumer, ServiceBusQueueConsumer>();
            services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
            services.AddTransient<IProcessData, ProcessData>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["Redis:ConnectionString"];
            });

            services.AddApplicationInsightsTelemetry();

            services.AddDbContext<BoilerplateDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("BoilerplateConnectionString")));
            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("BoilerplateConnectionString")));
            services.AddHangfireServer();

            services.AddControllers();
            services.AddApiVersioning();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            // register Consumer
            var azureConfig = app.ApplicationServices.GetService<AzureConfiguration>();
            var connectionString = azureConfig.ServiceBus.ConnectionString;
            var queueName = azureConfig.ServiceBus.QueueName;
            var topicName = azureConfig.ServiceBus.TopicName;
            if (!string.IsNullOrWhiteSpace(queueName) && !string.IsNullOrWhiteSpace(connectionString))
            {
                var bus = app.ApplicationServices.GetService<IServiceBusQueueConsumer>();
                bus.RegisterOnMessageHandlerAndReceiveMessages();
            }
            if (!string.IsNullOrWhiteSpace(topicName) && !string.IsNullOrWhiteSpace(connectionString))
            {
                var bus = app.ApplicationServices.GetService<IServiceBusTopicSubscription>();
                bus.RegisterOnMessageHandlerAndReceiveMessages();
            }
        }
    }
}
