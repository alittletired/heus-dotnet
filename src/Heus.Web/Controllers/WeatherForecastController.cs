using Microsoft.AspNetCore.Mvc;

namespace Heus.Web.Controllers;

/// <summary>
/// 测试枚举
/// </summary>
public enum TestEnum
{
    /// <summary>
    /// 测试1
    /// </summary>
    Test1,
    /// <summary>
    /// 测试3
    /// </summary>
    Test2,
    /// <summary>
    /// 测试3
    /// </summary>
    Test3
}
    public class WeatherForecast
    {
        public TestEnum TestEnum { get; set; } = TestEnum.Test1;
        public DateTime Date { get; set; }
/// <summary>
/// for text1
/// </summary>
        public int TemperatureC { get; set; }
/// <summary>
/// for text
/// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
