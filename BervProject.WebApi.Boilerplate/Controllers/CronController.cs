using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CronController : ControllerBase
    {
        [HttpPost("CreateCronOnce")]
        public IActionResult CreateCronOnce([FromServices] IBackgroundJobClient backgroundJobClient)
        {
            backgroundJobClient.Enqueue(() => Console.WriteLine("Hello World! Hangfire!"));
            return Ok();
        }

        [HttpPost("CreateRecurance")]
        public IActionResult CreateRecurance([FromServices] IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate("HelloWorld", () => Console.WriteLine("Hello World, Hourly! Hangfire!"), Cron.Hourly);
            return Ok();
        }
    }
}
