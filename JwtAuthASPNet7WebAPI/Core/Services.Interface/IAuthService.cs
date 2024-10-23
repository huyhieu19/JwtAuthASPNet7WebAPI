using JwtAuthASPNet7WebAPI.Core.Dtos;
using JwtAuthASPNet7WebAPI.Core.Entities;

namespace JwtAuthASPNet7WebAPI.Core.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto);
        Task<List<ApplicationRole>> GetRolesAsync();
        Task<AuthServiceResponseDto> AssignRoleAsync(AssignRoleDto dto);
        Task<AuthServiceResponseDto> ChangePasswordAsync(ChangePasswordDto dto);
        Task<AuthServiceResponseDto> ChangeInfoAsync(ChangeInfoDto dto);
    }
}
