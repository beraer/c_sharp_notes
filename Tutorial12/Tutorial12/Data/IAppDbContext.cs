using Microsoft.EntityFrameworkCore;
using Tutorial12.Models;

namespace APBDtut12.Data;

public interface IAppDbContext
{
    DbSet<Client> Clients { get; }
    
    DbSet<ClientTrip> ClientTrips { get; }
    DbSet<Country> Countries { get; }
    DbSet<Trip> Trips { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 