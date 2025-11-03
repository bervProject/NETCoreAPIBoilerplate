namespace BervProject.WebApi.Boilerplate.Services.AWS;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Amazon.S3;
using Amazon.S3.Model;

/// <inheritdoc />
public class AwsS3Service : IAwsS3Service
{
    private readonly IAmazonS3 _s3Client;
    /// <summary>
    /// Default constructor with dependency injections
    /// </summary>
    /// <param name="amazonS3"></param>
    public AwsS3Service(IAmazonS3 amazonS3)
    {
        _s3Client = amazonS3;
    }
    /// <summary>
    /// Implementation of Upload File
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    public async Task<string> UploadFile(IFormFile formFile)
    {
        var location = $"uploads/{formFile.FileName}";
        await using var stream = formFile.OpenReadStream();
        var putRequest = new PutObjectRequest
        {
            Key = location,
            BucketName = "upload-test-berv",
            InputStream = stream,
            AutoCloseStream = true,
            ContentType = formFile.ContentType
        };
        await _s3Client.PutObjectAsync(putRequest);
        return location;
    }
}