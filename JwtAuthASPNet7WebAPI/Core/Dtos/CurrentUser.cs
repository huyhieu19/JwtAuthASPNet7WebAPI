using JwtAuthASPNet7WebAPI.Core.OrtherObjects;

namespace JwtAuthASPNet7WebAPI.Core.Dtos
{
    public class CurrentUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<RoleType> Roles { get; set; }

        public CurrentUser(Guid id, string name, string email, string phoneNumber, List<RoleType> roles)
        {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Roles = roles;
        }
        public CurrentUser()
        {
            //this.SetDefault();
        }
        public void SetDefault()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Roles = [RoleType.Guest];
        }

    }

    public class RuntimeContextInstance
    {
        public CurrentUser User { get; set; }
    }
}
