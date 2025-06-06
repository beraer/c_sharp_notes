
using APBDtut12.Data;
using Microsoft.EntityFrameworkCore;
using Tutorial12.DTOs;

namespace Tutorial12.Services;

public class TripService : ITripService
{
    private readonly IAppDbContext _context;

    public TripService(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedTripsDTO> GetTripsAsync(int page, int pageSize)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

        var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.ClientTrips).ThenInclude(ct => ct.Client)
            .Include(t => t.CountryTrips).ThenInclude(ct => ct.Country)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new CountryDTO
                {
                    Name = ct.Country.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.Client.FirstName,
                    LastName = ct.Client.LastName
                }).ToList()
            }).ToListAsync();

        return new PagedTripsDTO
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = trips
        };
    }

    public async Task AddClientToTripAsync(int tripId, AddClientTripDTO addClientTrip)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == addClientTrip.Pesel);
        if (client != null)
        {
            bool alreadyRegistered = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == tripId);
            if (alreadyRegistered)
                throw new Exception("Client with given PESEL is already registered for this trip");
        }
        else
        {
            client = new Models.Client
            {
                FirstName = addClientTrip.FirstName,
                LastName = addClientTrip.LastName,
                Email = addClientTrip.Email,
                Telephone = addClientTrip.Telephone,
                Pesel = addClientTrip.Pesel
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }
        
        var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == tripId);
        if (trip == null)
            throw new Exception("Trip not found");
        if (trip.DateFrom < DateTime.Now)
            throw new Exception("Trip has already started");
        
        var clientTrip = new Models.ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = tripId,
            RegisteredAt = DateTime.Now,
            PaymentDate = addClientTrip.PaymentDate
        };
        _context.ClientTrips.Add(clientTrip);
        await _context.SaveChangesAsync();
    }
} 