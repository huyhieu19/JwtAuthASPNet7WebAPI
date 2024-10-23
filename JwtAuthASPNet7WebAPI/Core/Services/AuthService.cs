using JwtAuthASPNet7WebAPI.Core.Contexts;
using JwtAuthASPNet7WebAPI.Core.DbContext;
using JwtAuthASPNet7WebAPI.Core.Dtos;
using JwtAuthASPNet7WebAPI.Core.Entities;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using JwtAuthASPNet7WebAPI.Core.Services.Interface;
using JwtAuthASPNet7WebAPI.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthASPNet7WebAPI.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user is null)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };
            }
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordCorrect)
            {
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials"
                };
            }
            var authClaims = await Claims(user);
            string token = GenerateNewJsonWebToken(authClaims);
            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = token
            };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;
        }

        private async Task<List<Claim>> Claims(ApplicationUser user)
        {
            // Claim more values
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                new Claim("JWTID", Guid.NewGuid().ToString()),
            };
            // claim roles
            var roleIds = await _dbContext.UserRoles.Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .ToListAsync();
            var userRoles = await _dbContext.Roles.Where(r => roleIds.Contains(r.Id))
            .Select(r => new ReturnRoles
            {
                RoleType = r.RoleType.DescriptionAttribute(),
            }).Select(p => p.RoleType)
            .ToListAsync();
            var userRolesString = StaticExtentions.JoinRoles(userRoles);
            authClaims.Add(new Claim(ClaimTypes.Role, userRolesString ?? string.Empty));

            return authClaims;
        }
        private class ReturnRoles
        {
            public string RoleType { get; set; }
        }


        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistFirstsUser = await _userManager.FindByNameAsync(registerDto.FirstName);
            var isExistsLastUser = await _userManager.FindByNameAsync(registerDto.LastName);
            var isExistsUserName = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistFirstsUser != null && isExistFirstsUser != null && isExistsUserName != null && isExistFirstsUser == isExistsLastUser)
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists"
                };

            ApplicationUser newUser = new ApplicationUser()
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            // create new user
            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new AuthServiceResponseDto()
                {
                    IsSucceed = false,
                    Message = errorString
                };
            }

            // Add a Default USER Role to all users
            // Can change Role or add new role
            await _userManager.AddToRoleAsync(newUser, RoleType.Guest.DescriptionAttribute());
            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "User Created Successfully"
            };
        }

        public Task<List<ApplicationRole>> GetRolesAsync()
        {
            return _dbContext.Roles.ToListAsync();
        }

        public async Task<AuthServiceResponseDto> AssignRoleAsync(AssignRoleDto dto)
        {
            var lastUser = await _userManager.FindByIdAsync(dto.UserId.ToString()) ?? throw new Exception("User Not Found");
            await AddRole(dto.RoleType, dto.UserId);

            var newUser = await _userManager.FindByIdAsync(dto.UserId.ToString()) ?? throw new Exception("User Not Found");
            var authClaims = await Claims(newUser);
            string token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "New Token: " + token
            };

        }
        private async Task AddRole(RoleType roleType, Guid userId)
        {
            var roleName = roleType.DescriptionAttribute();

            // Check if the role exists
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new Exception($"Role {roleName} not found");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new Exception("User Not Found");

            // Check if the user already has this role
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                return;
            }

            // Add the user to the role
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to add role to user");
            }
        }

        public async Task<AuthServiceResponseDto> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email.ToString()) ?? throw new Exception("User Not Found");
            if (!UserContext.Current.User.Roles.Any(r => r >= RoleType.Admin))
            {
                if (UserContext.Current.User.Id.ToString() != user.Id)
                {
                    throw new Exception("Can not change password of other user");
                }
            }
            if (!await _userManager.CheckPasswordAsync(user, dto.OldPassword))
            {
                throw new Exception("Password Not Correct");
            };
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                throw new Exception("Password Not Changed");
            }
            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Password Changed Successfully"
            };

        }

        public async Task<AuthServiceResponseDto> ChangeInfoAsync(ChangeInfoDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString()) ?? throw new Exception("User Not Found");
            if (!UserContext.Current.User.Roles.Any(r => r >= RoleType.Admin))
            {
                if (UserContext.Current.User.Id.ToString() != user.Id)
                {
                    throw new Exception("Can not change infomation of other user");
                }
            }
            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            if (!dto.FirstName.IsNullOrEmpty())
            {
                user.FirstName = dto.FirstName;
            }
            if (!dto.LastName.IsNullOrEmpty())
            {
                user.LastName = dto.LastName;
            }
            if (!dto.UserName.IsNullOrEmpty())
            {
                user.UserName = dto.UserName;
            }
            if (!dto.Email.IsNullOrEmpty())
            {
                user.Email = dto.Email;
            }
            if (!dto.PhoneNumber.IsNullOrEmpty())
            {
                user.PhoneNumber = dto.PhoneNumber;
            }
            await _userManager.UpdateAsync(user);
            return new AuthServiceResponseDto()
            {
                IsSucceed = true,
                Message = "Info Changed Successfully"
            };
        }
    }
}
