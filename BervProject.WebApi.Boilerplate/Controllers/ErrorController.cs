using Microsoft.AspNetCore.Mvc;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public IActionResult Error() => Problem();
    }
}
