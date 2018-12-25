using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyCommute.Models
{
    public partial class MyCommuteContext : DbContext
    {
        public MyCommuteContext()
        {
        }

        public MyCommuteContext(DbContextOptions<MyCommuteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Fuel> Fuels { get; set; }
        public virtual DbSet<Ride> Rides { get; set; }
        public virtual DbSet<RidesUser> RidesUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=DESKTOP-2S88VU7;initial catalog=MyCommute;integrated security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.FuelType)
                    .IsRequired()
                    .HasConversion<int>();

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cars_Users");
            });

            modelBuilder.Entity<Fuel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FuelPrice).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.FuelType)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FromCity).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.ToCity).HasMaxLength(50);

                entity.Property(e => e.TravelDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<RidesUser>(entity =>
            {
                entity.HasKey(e => new { e.RideId, e.PassengerId });

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.RidesUsers)
                    .HasForeignKey(d => d.PassengerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RidesUsers_Users");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.RidesUsers)
                    .HasForeignKey(d => d.RideId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RidesUsers_Rides");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => new { e.Email, e.ProviderName })
                    .HasName("IX_Unique_Users")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.ProviderName).HasMaxLength(50);
            });
        }
    }
}
