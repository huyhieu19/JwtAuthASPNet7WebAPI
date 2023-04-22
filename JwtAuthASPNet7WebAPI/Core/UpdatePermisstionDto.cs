using System.ComponentModel.DataAnnotations;

namespace JwtAuthASPNet7WebAPI.Core
{
    public class UpdatePermisstionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
    }
}
