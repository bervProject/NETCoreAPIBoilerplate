using BervProject.WebApi.Boilerplate.Services.AWS;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DynamoController : ControllerBase
    {
        private readonly IDynamoDbServices _dynamoService;
        public DynamoController(IDynamoDbServices dynamoService)
        {
            _dynamoService = dynamoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            await _dynamoService.CreateObject();
            return Ok();
        }
    }
}
