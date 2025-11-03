namespace BervProject.WebApi.Boilerplate.Extensions;

using Amazon.DynamoDBv2;
using Amazon.S3;
using Amazon.SimpleEmail;
using Services.AWS;
using Microsoft.Extensions.DependencyInjection;


/// <summary>
/// AWS Extension for setup all AWS Services
/// </summary>
public static class SetupAwsExtension
{
    /// <summary>
    /// Setup AWS Services
    /// </summary>
    /// <param name="services"></param>
    public static void SetupAws(this IServiceCollection services)
    {
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDynamoDbServices, DynamoDbServices>();
        services.AddScoped<IAwsS3Service, AwsS3Service>();
    }
}

