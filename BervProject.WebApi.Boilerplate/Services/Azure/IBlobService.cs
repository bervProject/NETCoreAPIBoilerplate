namespace BervProject.WebApi.Boilerplate.Services.Azure;

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

/// <summary>
/// Azure Blob Service
/// </summary>
public interface IBlobService
{
    /// <summary>
    /// Create Container
    /// </summary>
    void CreateStorageContainer();
    /// <summary>
    /// Get Blobs Info
    /// </summary>
    /// <returns></returns>
    List<Dictionary<string, string>> GetBlobsInfo();
    /// <summary>
    /// Upload File
    /// </summary>
    /// <param name="formFile"></param>
    void UploadFile(IFormFile formFile);
}