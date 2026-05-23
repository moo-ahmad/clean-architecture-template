using ProjName.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ProjName.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            SeedIdentityRoles(builder);
        }

        private void SeedIdentityRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {

                    Id = Guid.Parse("4629D1F6-D91E-4E92-9B65-00CD28A10A06"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "59F0CD01-70B6-4DD5-A6D6-A41B8CA15BCA"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("98EDBC68-DE0A-47DC-A94F-3D3B76E9FC99"),
                    Name = "Sales",
                    NormalizedName = "SALES",
                    ConcurrencyStamp = "990C9461-B922-419A-AB90-D6223781A3D0"
                },
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("F38700B2-937B-4155-8819-738291E02DB5"),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "73356855-2611-4F6C-8E11-6D4589D80153"
                }
            );
        }
    }
}
