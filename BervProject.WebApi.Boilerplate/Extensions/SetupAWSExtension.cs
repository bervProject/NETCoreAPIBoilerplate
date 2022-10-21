namespace BervProject.WebApi.Boilerplate.Extenstions;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.SimpleEmail;
using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.Extensions.DependencyInjection;

public static class SetupAWSExtension
{
    public static void SetupAWS(this IServiceCollection services)
    {
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDynamoDbServices, DynamoDbServices>();
        services.AddScoped<IAWSS3Service, AWSS3Service>();
    }
}

