using BervProject.WebApi.Boilerplate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BervProject.WebApi.Boilerplate.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController()
        {
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("cache")]
        public IActionResult GetCache([FromServices] IDistributedCache distributedCache)
        {
            var result = distributedCache.Get("MyCache");
            if (result == null || result.Length == 0)
            {
                var utf8 = new UTF8Encoding();
                distributedCache.Set("MyCache", utf8.GetBytes("CustomCache"));
                result = distributedCache.Get("MyCache");
                Console.WriteLine(result);
            }
            return Ok(result);
        }
    }
}
