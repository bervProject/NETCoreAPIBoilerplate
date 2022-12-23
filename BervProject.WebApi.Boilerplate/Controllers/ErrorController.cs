using Microsoft.AspNetCore.Mvc;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Error Controller
        /// </summary>
        /// <returns></returns>
        [Route("error")]
        [HttpGet]
        public IActionResult Error() => Problem();
    }
}
