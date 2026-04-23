using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole 
                {  
                    Id = "b9814199-7c17-41be-94e1-8fc20fb40230",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "1"
                },
                new IdentityRole 
                {
                    Id = "7656f3b9-4def-48a4-a897-8e3cb4f2600a",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR",
                    ConcurrencyStamp = "2"
                },
                new IdentityRole 
                {
                    Id = "52f87679-a914-475a-85a5-751ae37def2e",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "3"
                }
            );

            //var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
                {
                    Id = "2117946d-7479-4511-a6ed-7ac18c5036de",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEFgcZPJY56FWtyEsQlwT2/r/ZOpz5D7u2HnTadem5MSYrFdlEZBGuTeep3NJr35Z+g==",
                    // PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),
                    EmailConfirmed = true,
                    ConcurrencyStamp = "4",
                    SecurityStamp = "5",
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1999,12,12)
            });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "52f87679-a914-475a-85a5-751ae37def2e",
                    UserId = "2117946d-7479-4511-a6ed-7ac18c5036de"
                });
        }

        public DbSet<LeaveType> LeaveTypes { get; set; } // LeaveTypes: name of the table in db
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Period> Periods { get; set; }
    }
}
