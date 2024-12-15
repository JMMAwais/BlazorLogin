using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorLogin.Data.Context
{
    public class DbContext : IdentityDbContext<IdentityUser>
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed default user
            var hasher = new PasswordHasher<IdentityUser>();

            var adminUser = new IdentityUser
            {
                Id = "1",
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = Guid.NewGuid().ToString()
            };

            builder.Entity<IdentityUser>().HasData(adminUser);
        }
    }
}