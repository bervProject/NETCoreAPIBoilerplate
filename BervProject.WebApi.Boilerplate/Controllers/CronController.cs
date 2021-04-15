using BervProject.WebApi.Boilerplate.Services;
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
            backgroundJobClient.Enqueue<ICronService>((x) => x.HelloWorld());
            return Ok();
        }

        [HttpPost("CreateRecurance")]
        public IActionResult CreateRecurance([FromServices] IRecurringJobManager recurringJobManager)
        {
            recurringJobManager.AddOrUpdate<ICronService>("HelloWorld", (x) => x.HelloWorld(), Cron.Hourly);
            return Ok();
        }
    }
}
