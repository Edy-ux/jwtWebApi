using Microsoft.EntityFrameworkCore;
using jwtWebApi.Models;
using jwtWebApi.Services;

namespace TypesfRelationships.Context
{
    public class AppDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<User>()
               .Property(x => x.Login).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasMaxLength(13).IsRequired();

            modelBuilder.Entity<User>()
                .HasMany(x => x.RefreshTokens)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }

    }
}
