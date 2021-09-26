using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BervProject.WebApi.Boilerplate.Services.AWS
{
  public interface IAWSS3Service
  {
    Task<string> UploadFile(IFormFile formFile);
  }
}
