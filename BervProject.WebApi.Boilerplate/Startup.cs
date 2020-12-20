using BervProject.WebApi.Boilerplate.ConfigModel;
using BervProject.WebApi.Boilerplate.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IDynamoDbServices, DynamoDbServices>();
            services.AddScoped<IQueueServices, QueueServices>();
            services.AddScoped<ITopicServices, TopicServices>();

            services.AddSingleton<IServiceBusQueueConsumer, ServiceBusQueueConsumer>();
            services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
            services.AddTransient<IProcessData, ProcessData>();

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
