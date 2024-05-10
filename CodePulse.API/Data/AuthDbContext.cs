using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "8533fa19-7769-42ca-80bb-05656f588a9d";
            var writerRoleId = "d99442ff-7eb6-46f2-9861-950f7553112b";
            // Create Reader and Writer Role
            var roles = new List<IdentityRole> {
                new IdentityRole(){
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole(){
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Create Admin - (Default User)
            var adminUserId = "da717d2c-4e1f-4b66-8179-18f5b98c8b9d";
            var admin = new IdentityUser() {
                Id = adminUserId,
                UserName = "admin@codepulse.com",
                Email = "admin@codepulse.com",
                NormalizedEmail = "admin@codepulse.com".ToUpper(),
                NormalizedUserName = "admin@codepulse.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>(){
                new(){
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new(){
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}