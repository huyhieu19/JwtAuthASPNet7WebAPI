using System.ComponentModel.DataAnnotations;

namespace JwtAuthASPNet7WebAPI.Core.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
    }
}
