using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthASPNet7WebAPI.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Get")]

        public IActionResult Get()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("Getbyuser")]
        [Authorize(Roles = StaticUserRoles.USER)]
        public IActionResult GetbyUser()
        {
            return Ok(Summaries);
        }
        
        [HttpGet]
        [Route("Getbyadmin")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public IActionResult GetbyAdmin()
        {
            return Ok(Summaries);
        }
        
        [HttpGet]
        [Route("Getbyowner")]
        [Authorize(Roles = StaticUserRoles.OWNER)]
        public IActionResult GetbyOwner()
        {
            return Ok(Summaries);
        }
    }
}