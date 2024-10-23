using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using Microsoft.AspNetCore.Identity;

namespace JwtAuthASPNet7WebAPI.Core.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public RoleType RoleType { get; set; }
    }
}
