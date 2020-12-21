using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BervProject.WebApi.Boilerplate.Services.Azure
{
    public interface IBlobService
    {
        void CreateStorageContainer();
        List<Dictionary<string, string>> GetBlobsInfo();
        void UploadFile(IFormFile formFile);
    }
}
