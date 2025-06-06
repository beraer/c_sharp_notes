using APBDtut12.Data;
using Microsoft.EntityFrameworkCore;
using Tutorial12.Models;


namespace Tutorial12.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<ClientTrip> ClientTrips { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Trip> Trips { get; set; }
    public virtual DbSet<CountryTrip> CountryTrips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().ToTable("Client");
        modelBuilder.Entity<Trip>().ToTable("Trip");
        modelBuilder.Entity<Country>().ToTable("Country");
        modelBuilder.Entity<ClientTrip>().ToTable("Client_Trip");
        modelBuilder.Entity<CountryTrip>().ToTable("Country_Trip");

        modelBuilder.Entity<ClientTrip>()
            .HasKey(ct => new { ct.IdClient, ct.IdTrip });

        modelBuilder.Entity<ClientTrip>()
            .HasOne(ct => ct.Client)
            .WithMany(c => c.ClientTrips)
            .HasForeignKey(ct => ct.IdClient);

        modelBuilder.Entity<ClientTrip>()
            .HasOne(ct => ct.Trip)
            .WithMany(t => t.ClientTrips)
            .HasForeignKey(ct => ct.IdTrip);

        modelBuilder.Entity<CountryTrip>()
            .HasKey(ct => new { ct.IdCountry, ct.IdTrip });

        modelBuilder.Entity<CountryTrip>()
            .HasOne(ct => ct.Country)
            .WithMany(c => c.CountryTrips)
            .HasForeignKey(ct => ct.IdCountry);

        modelBuilder.Entity<CountryTrip>()
            .HasOne(ct => ct.Trip)
            .WithMany(t => t.CountryTrips)
            .HasForeignKey(ct => ct.IdTrip);
    }
} 