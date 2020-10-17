using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Models;
using TravianAnalytics.Models.CronJob;
using TravianAnalytics.Models.Identity.Abstract;
using TravianAnalytics.Models.Player;

namespace TravianAnalytics.Data {
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken> {
        public DbSet<Alliance> Alliances { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CronJob> CronJobs { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            // Override the database collation
            builder.UseCollation("Latin1_General_CI_AI_WS");

            builder.Entity<User>().HasData(new List<User>() {
                new User() {
                    Active = true,
                    Name = "Daniel Oliveira",
                    Id = 1,
                    Email = "daniel-oliveira-11@hotmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = true,
                    ConcurrencyStamp = "2d21b3a5-eb27-4ff0-9220-d61b6f0bdc09",
                    NormalizedEmail = "DANIEL-OLIVEIRA-11@HOTMAIL.COM",
                    PasswordHash = "AQAAAAEAACcQAAAAEMT8/6ikxCfMHguP7I5flL8LZq/nO6tjuXFwnF5nM3pmHjbV7b/Cz2/HEjsA0oswUg==",
                    UserName = "DanielMuscleBoy",
                    NormalizedUserName = "DANIELMUSCLEBOY",
                    SecurityStamp = "TUARGYDNHGFROWIZUR6OEGD5YQDNRNKB",
                    Master = true
                }
            });

            // Override identity configuration and table names
            builder.Entity<User>(b => {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims).WithOne(e => e.User).HasForeignKey(uc => uc.UserId).IsRequired();
                // Each User can have many UserLogins
                b.HasMany(e => e.Logins).WithOne(e => e.User).HasForeignKey(ul => ul.UserId).IsRequired();
                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens).WithOne(e => e.User).HasForeignKey(ut => ut.UserId).IsRequired();

            });

            // Rename identity tables
            builder.Entity<User>(b => b.ToTable("Users"));
            builder.Entity<Role>(b => b.ToTable("Roles"));
            builder.Entity<RoleClaim>(b => b.ToTable("RoleClaims"));
            builder.Entity<UserClaim>(b => b.ToTable("UserClaims"));
            builder.Entity<UserLogin>(b => b.ToTable("UserLogins"));
            builder.Entity<UserToken>(b => b.ToTable("UserTokens"));
            builder.Entity<UserRole>(b => b.ToTable("UserRoles"));
        }

    }
}
