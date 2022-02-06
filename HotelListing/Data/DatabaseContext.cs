using HotelListing.Configurations.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext: IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Hotel> Hotels { get; set; }

        // Seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // added to support IdentityDbContext
            base.OnModelCreating(modelBuilder);

            // seed the roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "User", NormalizedName = "USER" },
                new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
            );

            // seed the Country
            modelBuilder.Entity<Country>().HasData(
                new Country() { Id = 1, Name = "Jamaica", ShortName = "JM" },
                new Country() { Id = 2, Name = "Bahamas", ShortName = "BS" },
                new Country() { Id = 3, Name = "Cayman Island", ShortName = "CI" }
            );

            // seed the Hotel
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel() { Id = 1, Name = "Sandals Resort and Spa", Address = "Negril", CountryId = 1, Rating = 4.5 },
                new Hotel() { Id = 2, Name = "Comfort Suits", Address = "George Town", CountryId = 3, Rating = 4.5 },
                new Hotel() { Id = 3, Name = "Grand Palladium", Address = "Nassua", CountryId = 2, Rating = 4.3 }
            );
        }

    }
}
