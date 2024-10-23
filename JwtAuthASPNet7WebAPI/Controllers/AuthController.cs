using JwtAuthASPNet7WebAPI.Core.Contexts;
using JwtAuthASPNet7WebAPI.Core.CusAuthorizeAttribute;
using JwtAuthASPNet7WebAPI.Core.Dtos;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using JwtAuthASPNet7WebAPI.Core.Services.Interface;
using JwtAuthASPNet7WebAPI.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthASPNet7WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // router for seeding my roles to DB
        [HttpPost]
        [Route("all-roles")]
        public async Task<IActionResult> AllRoles()
        {
            return Ok(await _authService.GetRolesAsync());
        }

        [HttpPost]
        [Route("information")]
        public async Task<IActionResult> Information()
        {
            return Ok($"Name: {UserContext.Current.User.Name}" +
                $"\nEmail: {UserContext.Current.User.Email}" +
                $"\nRole: {UserContext.Current.User.Roles.JoinRoles()}" +
                $"\nId: {UserContext.Current.User.Id}");
        }


        // route -> register

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return Ok(await _authService.RegisterAsync(registerDto));
        }

        // Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return Ok(await _authService.LoginAsync(loginDto));
        }

        // Create Role for user
        // Route role user -> Admin
        [HttpPost]
        [Route("assign-role")]
        //[CusAuthorize(RoleType.Owner | RoleType.Admin)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            return Ok(await _authService.AssignRoleAsync(dto));
        }

        // Create Role owner for user
        // Route -> make user -> owner
        [HttpPost]
        [Route("modify-information")]
        [CusAuthorize(RoleType.User | RoleType.Owner | RoleType.Admin)]
        public async Task<IActionResult> ModifyInformation([FromBody] ChangeInfoDto dto)
        {
            return Ok(await _authService.ChangeInfoAsync(dto));
        }

        [HttpPost]
        [Route("modify-password")]
        [CusAuthorize(RoleType.User | RoleType.Owner | RoleType.Admin)]
        public async Task<IActionResult> ModifyPassword([FromBody] ChangePasswordDto dto)
        {
            return Ok(await _authService.ChangePasswordAsync(dto));
        }
    }
}
