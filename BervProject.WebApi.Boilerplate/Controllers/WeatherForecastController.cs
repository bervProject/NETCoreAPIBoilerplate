﻿using BervProject.WebApi.Boilerplate.Entities;
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

        [HttpGet]
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

        [HttpGet("cache")]
        public ActionResult<byte[]> GetCache([FromServices] IDistributedCache distributedCache)
        {
            var result = distributedCache.Get("MyCache");
            if (result == null || result.Length == 0)
            {
                var utf8 = new UTF8Encoding();
                distributedCache.Set("MyCache", utf8.GetBytes("CustomCache"));
                result = distributedCache.Get("MyCache");
                Console.WriteLine(result);
            }
            return result;
        }

        [HttpGet("db")]
        public IActionResult GetDb([FromServices] BoilerplateDbContext dbContext)
        {
            var booksQuery = dbContext.Books.AsQueryable();
            var books = booksQuery.Where(x => x.Name.Equals("Halleluya")).Include(x => x.Publisher).ToList();
            if (books.Count == 0)
            {
                var listBooks = new List<Book>()
                {
                    new Book()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Halleluya",
                        Publisher = new Publisher()
                        {
                            Id = Guid.NewGuid(),
                            Name = "Heaven Publisher"
                        }
                    },
                    new Book()
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
            else
            {
                return Ok(books);
            }
        }

        [HttpGet("triggerException")]
        public IActionResult TriggerException()
        {
            throw new Exception("Unhandled Exception");
        }
    }
}
