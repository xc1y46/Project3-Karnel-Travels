using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace KarlanTravels_Adm.Models
{
    public partial class ContextModel : DbContext
    {
        public ContextModel()
            : base("name=ContextModel")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<AdminRole> AdminRole { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<FacilityType> FacilityType { get; set; }
        public virtual DbSet<Tour> Tour { get; set; }
        public virtual DbSet<TourDetail> TourDetail { get; set; }
        public virtual DbSet<TouristSpot> TouristSpot { get; set; }
        public virtual DbSet<TransactionRecord> TransactionRecord { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasMany(e => e.TransactionRecord)
                .WithRequired(e => e.Admin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AdminRole>()
                .HasMany(e => e.Admin)
                .WithRequired(e => e.AdminRole)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.TouristSpot)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.Customer)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.Facility)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.TouristSpot)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.City)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.AmountToPay)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.TransactionRecord)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Facility>()
                .Property(e => e.FacilityPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.TourDetail)
                .WithRequired(e => e.Facility)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FacilityType>()
                .HasMany(e => e.Facility)
                .WithRequired(e => e.FacilityType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tour>()
                .Property(e => e.TourPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Tour>()
                .HasMany(e => e.TourDetail)
                .WithRequired(e => e.Tour)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tour>()
                .HasMany(e => e.TransactionRecord)
                .WithRequired(e => e.Tour)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TouristSpot>()
                .HasMany(e => e.TourDetail)
                .WithRequired(e => e.TouristSpot)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionRecord>()
                .Property(e => e.TransactionFee)
                .HasPrecision(19, 4);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.TransactionRecord)
                .WithRequired(e => e.TransactionType)
                .WillCascadeOnDelete(false);
        }
    }
}
