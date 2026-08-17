namespace BervProject.WebApi.Boilerplate.Services.AWS;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

/// <summary>
/// AWS S3 Service Interface
/// </summary>
public interface IAwsS3Service
{
    /// <summary>
    /// Upload File
    /// </summary>
    /// <param name="formFile"></param>
    /// <returns></returns>
    Task<string> UploadFile(IFormFile formFile);
}