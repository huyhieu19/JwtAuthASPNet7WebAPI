using JwtAuthASPNet7WebAPI.Core.Contexts;
using JwtAuthASPNet7WebAPI.Core.CusAuthorizeAttribute;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using JwtAuthASPNet7WebAPI.Core.Utils;
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
        [CusAuthorize(RoleType.User)]
        public IActionResult GetbyUser()
        {
            return Ok("Name: " + UserContext.Current.User.Name +
                "\nEmail: " + UserContext.Current.User.Email +
                "\nRole: " + UserContext.Current.User.Roles.JoinRoles() +
                "\nId: " + UserContext.Current.User.Id);
        }

        [HttpGet]
        [Route("Getbyadmin")]
        [CusAuthorize(RoleType.User)]
        public IActionResult GetbyAdmin()
        {
            return Ok(Summaries);
        }

        [HttpGet]
        [Route("Getbyowner")]
        [CusAuthorize(RoleType.User)]
        public IActionResult GetbyOwner()
        {
            return Ok(Summaries);
        }
    }
}