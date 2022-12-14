using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        private readonly string _connectionString = "Server=DESKTOP-6RAT31V;Database=RestaurantsDb;Trusted_Connection=True;";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name).IsRequired().HasMaxLength(25);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Address>()
                .Property(a => a.City).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Dish>()
                .Property(d => d.Name).IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
