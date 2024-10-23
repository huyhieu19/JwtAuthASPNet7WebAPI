using JwtAuthASPNet7WebAPI.Core.DbContext.Configurations;
using JwtAuthASPNet7WebAPI.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthASPNet7WebAPI.Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.ApplyConfiguration(new RoleConfiguration());
        }

        public virtual DbSet<ApplicationUser> Users { get; set; } = default!;
        public virtual DbSet<ApplicationRole> Roles { get; set; } = default!;

    }
}
