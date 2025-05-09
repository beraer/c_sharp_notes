using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public class TripsService : ITripsService
    {
        private readonly string _conn;

        public TripsService(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<TripDTO>> GetTrips()
        {
            var trips = new List<TripDTO>();
            using var c = new SqlConnection(_conn);
            await c.OpenAsync();
            
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
                    FROM Trip";
                using var r = await cmd.ExecuteReaderAsync();
                while (await r.ReadAsync())
                {
                    trips.Add(new TripDTO {
                        IdTrip      = r.GetInt32(0),
                        Name        = r.GetString(1),
                        Description = r.IsDBNull(2) ? null : r.GetString(2),
                        DateFrom    = r.GetDateTime(3),
                        DateTo      = r.GetDateTime(4),
                        MaxPeople   = r.GetInt32(5),
                        Countries   = new List<CountryDTO>()
                    });
                }
            }

            if (trips.Count == 0) return trips;
            
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT ct.IdTrip, c.IdCountry, c.Name
                    FROM Country_Trip ct
                    JOIN Country c ON ct.IdCountry = c.IdCountry";
                using var r = await cmd.ExecuteReaderAsync();
                while (await r.ReadAsync())
                {
                    int tripId = r.GetInt32(0);
                    var trip = trips.Find(t => t.IdTrip == tripId);
                    trip?.Countries.Add(new CountryDTO {
                        IdCountry = r.GetInt32(1),
                        Name      = r.GetString(2)
                    });
                }
            }

            return trips;
        }

        public async Task<TripDTO> GetTripById(int id)
        {
            TripDTO trip = null;
            using var c = new SqlConnection(_conn);
            await c.OpenAsync();
            
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT IdTrip, Name, Description, DateFrom, DateTo, MaxPeople
                    FROM Trip
                    WHERE IdTrip = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using var r = await cmd.ExecuteReaderAsync();
                if (!await r.ReadAsync()) return null;
                trip = new TripDTO {
                    IdTrip      = r.GetInt32(0),
                    Name        = r.GetString(1),
                    Description = r.IsDBNull(2) ? null : r.GetString(2),
                    DateFrom    = r.GetDateTime(3),
                    DateTo      = r.GetDateTime(4),
                    MaxPeople   = r.GetInt32(5),
                    Countries   = new List<CountryDTO>()
                };
            }

            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT c.IdCountry, c.Name
                    FROM Country_Trip ct
                    JOIN Country c ON ct.IdCountry = c.IdCountry
                    WHERE ct.IdTrip = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using var r = await cmd.ExecuteReaderAsync();
                while (await r.ReadAsync())
                {
                    trip.Countries.Add(new CountryDTO {
                        IdCountry = r.GetInt32(0),
                        Name      = r.GetString(1)
                    });
                }
            }

            return trip;
        }
    }
}
