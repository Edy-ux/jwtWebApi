using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace jwtWebApi.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class WheaterForecastController : ControllerBase
{

    private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];


    [HttpGet(Name = "GetWeatherForacast"), Authorize(Roles = "Admin, User, Student")]
    public IEnumerable<WheaterForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WheaterForecast
        {
            TemperatureC = Random.Shared.Next(-20, 30),
            Date = DateTime.Now.AddDays(index),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]

        });
    }
}



