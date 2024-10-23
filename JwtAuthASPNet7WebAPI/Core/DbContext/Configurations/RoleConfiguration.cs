using JwtAuthASPNet7WebAPI.Core.Entities;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using JwtAuthASPNet7WebAPI.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthASPNet7WebAPI.Core.DbContext.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleType.Admin.DescriptionAttribute(),
                NormalizedName = RoleType.Admin.DescriptionAttribute().ToUpper(),
                RoleType = RoleType.Admin
            },
            new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleType.Owner.DescriptionAttribute(),
                NormalizedName = RoleType.Owner.DescriptionAttribute().ToUpper(),
                RoleType = RoleType.Owner
            },
            new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleType.User.DescriptionAttribute(),
                NormalizedName = RoleType.User.DescriptionAttribute().ToUpper(),
                RoleType = RoleType.User
            },
            new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = RoleType.Guest.DescriptionAttribute(),
                NormalizedName = RoleType.Guest.DescriptionAttribute().ToUpper(),
                RoleType = RoleType.Guest
            });
        }
    }
}
