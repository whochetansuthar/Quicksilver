using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Quicksilver.DAL.QuicksilverDbContext
{
    public partial class QuicksilverContext : DbContext
    {
        public QuicksilverContext(DbContextOptions<QuicksilverContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Cargoes> Cargoes { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Coupons> Coupons { get; set; }
        public virtual DbSet<CourierTypes> CourierTypes { get; set; }
        public virtual DbSet<Feedbacks> Feedbacks { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Rates> Rates { get; set; }
        public virtual DbSet<States> States { get; set; }
        public virtual DbSet<Stations> Stations { get; set; }
        public virtual DbSet<TrackingHistory> TrackingHistory { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=WINDOWSPC;Database=Quicksilver;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargoes>(entity =>
            {
                entity.Property(e => e.CargoCompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cities>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Coupons>(entity =>
            {
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateExpired).HasColumnType("datetime");
            });

            modelBuilder.Entity<CourierTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Feedbacks>(entity =>
            {
                entity.Property(e => e.DateGiven).HasColumnType("date");

                entity.Property(e => e.Review)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<States>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("IX_States")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Stations>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrackingHistory>(entity =>
            {
                entity.Property(e => e.DateDispatched).HasColumnType("datetime");

                entity.Property(e => e.DateRecorded).HasColumnType("datetime");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Cgst).HasColumnName("CGST");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Sgst).HasColumnName("SGST");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.AspNetUserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
