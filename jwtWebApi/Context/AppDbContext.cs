using Microsoft.EntityFrameworkCore;

namespace TypesfRelationships.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<HouseModel>()
            //    .WithOne(e => e.Address)
            //    .HasForeignKey(e => e.AddressId)
            //    .HasPrincipalKey(e => e.Id);
        }

    }
}
