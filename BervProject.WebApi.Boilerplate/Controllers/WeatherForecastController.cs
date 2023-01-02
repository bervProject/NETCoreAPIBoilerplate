using BervProject.WebApi.Boilerplate.Entities;
using BervProject.WebApi.Boilerplate.EntityFramework;
using BervProject.WebApi.Boilerplate.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

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

        private readonly TelemetryClient telemetryClient;

        public WeatherForecastController(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Get Weather Forecast
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<WeatherForecast>), StatusCodes.Status200OK)]
        public IEnumerable<WeatherForecast> Get()
        {
            this.telemetryClient.TrackEvent("WeatherQueried");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Get Cache
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <returns></returns>
        [HttpGet("cache")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult<string> GetCache([FromServices] IDistributedCache distributedCache)
        {
            var result = distributedCache.Get("MyCache");
            if (result == null || result.Length == 0)
            {
                var utf8 = new UTF8Encoding();
                distributedCache.Set("MyCache", utf8.GetBytes("CustomCache"));
                result = distributedCache.Get("MyCache");
                Console.WriteLine(result);
            }
            return Encoding.UTF8.GetString(result ?? Array.Empty<byte>());
        }

        /// <summary>
        /// Simple DB Query
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        [HttpGet("db")]
        [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
        public ActionResult<List<Book>> GetDb([FromServices] BoilerplateDbContext dbContext)
        {
            var booksQuery = dbContext.Books.AsQueryable();
            var books = booksQuery.Where(x => x.Name.Contains("Halleluya")).Include(x => x.Publisher).ToList();
            if (books.Count == 0)
            {
                var listBooks = new List<Book>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Halleluya",
                        Publisher = new Publisher()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Heaven Publisher"
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Halleluya 2",
                        Publisher = new Publisher()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Heaven Publisher"
                        }
                    }
                };
                dbContext.Books.AddRange(listBooks);
                dbContext.SaveChanges();
                return Ok(listBooks);
            }

            return Ok(books);
        }

        /// <summary>
        /// Exception always throw
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("triggerException")]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        public IActionResult TriggerException()
        {
            throw new Exception("Unhandled Exception");
        }
    }
}
