using BervProject.WebApi.Boilerplate.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
