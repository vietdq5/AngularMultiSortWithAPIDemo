using AdminHRM.Server.DataContext;
using AdminHRM.Server.Entities;
using ElasticsearchExampleDemo.Server.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElasticsearchExampleDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly HrmDbContext _hrmDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, HrmDbContext hrmDbContext)
        {
            _logger = logger;
            _hrmDbContext = hrmDbContext;
        }

        [HttpGet("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees([FromQuery] List<SortColumn> sortColumns)
        {
            // Fetch data from your data source (e.g., database)
            var query = _hrmDbContext.Employees.AsQueryable();

            // Apply sorting
            if (sortColumns != null && sortColumns.Count > 0)
            {
                query = query.OrderByDynamic<Employee>(sortColumns);
            }
            var data = await query
                .AsNoTracking()
                .Select(s => new
                {
                    s.FirstName,
                    s.LastName,
                    s.JobTitle
                })
                .ToListAsync();
            return Ok(data);
        }
    }

    public class SortColumn
    {
        public string Column { get; set; }
        public string Direction { get; set; }
    }
}