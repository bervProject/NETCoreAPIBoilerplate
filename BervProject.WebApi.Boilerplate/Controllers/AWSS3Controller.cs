using System.Threading.Tasks;
using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SThreeController : ControllerBase
    {
        private readonly IAWSS3Service _awsS3Service;
        public SThreeController(IAWSS3Service awsS3Service)
        {
            _awsS3Service = awsS3Service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var result = await _awsS3Service.UploadFile(file);
            return Ok(new
            {
                path = result
            });
        }
    }
}
