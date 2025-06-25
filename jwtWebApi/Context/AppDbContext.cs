using Microsoft.EntityFrameworkCore;
using jwtWebApi.Models;


namespace jwtWebApi.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(
            DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<User>()
               .Property(x => x.Login)
               .HasMaxLength(128).IsRequired();

            modelBuilder.Entity<User>().Property(x => x.PasswordHash).HasMaxLength(70).IsRequired();

            // Relationships  between User and RefreshToken
            modelBuilder.Entity<User>()
             .HasMany(u => u.RefreshTokens)                   // Propriedade de navegação
             .WithOne(rt => rt.User)                          // Propriedade de navegação inversa
             .HasForeignKey(rt => rt.UserId);                 // FK explícita

            modelBuilder.Entity<User>()
                .Metadata
                .FindNavigation(nameof(User.RefreshTokens))!
                .SetPropertyAccessMode(PropertyAccessMode.Field); // Usa o campo privado

            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);



        }

    }
}
