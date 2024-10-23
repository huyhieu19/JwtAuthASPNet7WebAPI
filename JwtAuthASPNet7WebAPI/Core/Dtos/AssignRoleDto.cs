using JwtAuthASPNet7WebAPI.Core.OrtherObjects;

namespace JwtAuthASPNet7WebAPI.Core.Dtos
{
    public class AssignRoleDto
    {
        public Guid UserId { get; set; }
        public RoleType RoleType { get; set; }
    }
}
