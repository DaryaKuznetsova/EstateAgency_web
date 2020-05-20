namespace EstateAgency
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Agency : DbContext
    {
        public Agency()
            : base("name=Agency")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<EstateObject> EstateObjects { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<PaymentInstrument> PaymentInstruments { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<PictureObjectLink> PictureObjectLinks { get; set; }
        public virtual DbSet<RealtyType> RealtyTypes { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }
        public virtual DbSet<TradeType> TradeTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Trades)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<District>()
                .HasMany(e => e.EstateObjects)
                .WithRequired(e => e.District)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EstateObject>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.EstateObject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Manager>()
                .HasMany(e => e.Trades)
                .WithRequired(e => e.Manager)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Owner>()
                .HasMany(e => e.EstateObjects)
                .WithRequired(e => e.Owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RealtyType>()
                .HasMany(e => e.EstateObjects)
                .WithRequired(e => e.RealtyType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Status>()
                .HasMany(e => e.EstateObjects)
                .WithRequired(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trade>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.Trade)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TradeType>()
                .HasMany(e => e.EstateObjects)
                .WithRequired(e => e.TradeType)
                .WillCascadeOnDelete(false);
        }
    }
}
