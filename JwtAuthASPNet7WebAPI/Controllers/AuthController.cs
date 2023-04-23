using JwtAuthASPNet7WebAPI.Core.Dtos;
using JwtAuthASPNet7WebAPI.Core.Entities;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using JwtAuthASPNet7WebAPI.Core.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        [Route("seed-role")]
        public async Task<IActionResult> SeedRoles()
        {
            return Ok(await _authService.SeedRolesAsync());
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
        [Route("marke-admin")]
        public async Task<IActionResult> MarkeAdmin([FromBody]UpdatePermissionDto updatePermisstionDto)
        {
            return Ok(await _authService.MakeAdminAsync(updatePermisstionDto));
        }
        
        // Create Role owner for user
        // Route -> make user -> owner
        [HttpPost]
        [Route("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermisstionDto)
        {
            return Ok(await _authService.MakeOwnerAsync(updatePermisstionDto));
        }
    }
}
