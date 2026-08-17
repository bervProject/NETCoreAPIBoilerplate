namespace BervProject.WebApi.Boilerplate.Models;

using System;

/// <summary>
/// Weather Forecast
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Temperature in Celsius
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in F
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Summary
    /// </summary>
    public string Summary { get; set; }
}