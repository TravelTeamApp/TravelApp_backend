using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<VisitedPlace> VisitedPlaces { get; set; }

        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<PlaceType> PlaceTypes { get; set; } 
        public DbSet<UserPlaceType> UserPlaceTypes { get; set; }  // Yeni ilişki tablosu

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Favorite entity için ilişki tanımlaması
            builder.Entity<Favorite>()
                .HasKey(f => f.FavoriteId);
            //builder.Entity<Favorite>(x => x.HasKey(p => new { p.UserID, p.PlaceId }));

            builder.Entity<Favorite>()
                .HasOne(u => u.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(p => p.UserID);

            builder.Entity<Favorite>()
                .HasOne(u => u.Place)
                .WithMany(u => u.Favorites)
                .HasForeignKey(p => p.PlaceId);

            // Favorite entity için ilişki tanımlaması
            builder.Entity<VisitedPlace>()
                .HasKey(f => f.VisitedPlaceId);
            //builder.Entity<VisitedPlaces>(x => x.HasKey(p => new { p.UserID, p.PlaceId }));

            builder.Entity<VisitedPlace>()
                .HasOne(u => u.User)
                .WithMany(u => u.VisitedPlaces)
                .HasForeignKey(p => p.UserID);

            builder.Entity<VisitedPlace>()
                .HasOne(u => u.Place)
                .WithMany(u => u.VisitedPlaces)
                .HasForeignKey(p => p.PlaceId);

            // Place ve PlaceType arasında bire bir ilişki kuruluyor
            builder.Entity<Place>()
                .HasOne(p => p.PlaceType)
                .WithMany()
                .HasForeignKey(p => p.PlaceTypeId);

            // User ile PlaceType arasında çoktan çoğa ilişki
            builder.Entity<UserPlaceType>()
                .HasKey(ust => new { ust.UserId, ust.PlaceTypeId });
            
            builder.Entity<UserPlaceType>()
                .HasOne(ust => ust.User)
                .WithMany(u => u.UserPlaceTypes)
                .HasForeignKey(ust => ust.UserId);

            builder.Entity<UserPlaceType>()
                .HasOne(ust => ust.PlaceType)
                .WithMany(pt => pt.UserPlaceTypes)
                .HasForeignKey(ust => ust.PlaceTypeId);
        }
    }
}
