using System.ComponentModel;

namespace JwtAuthASPNet7WebAPI.Core.OrtherObjects
{
    public enum RoleType
    {
        [Description("Guest")]
        Guest = 1,
        [Description("User")]
        User = Guest << 1,
        [Description("Admin")]
        Admin = User << 1,
        [Description("Owner")]
        Owner = Admin << 1
    }
}
