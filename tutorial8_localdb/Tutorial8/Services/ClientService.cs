
using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public class ClientsService : IClientsService
    {
        private readonly string _conn;
        public ClientsService(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        private async Task<bool> ClientExists(SqlConnection connection, int clientId)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Client WHERE IdClient = @id";
            cmd.Parameters.AddWithValue("@id", clientId);
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }
        
        private async Task<bool> TripExists(SqlConnection connection, int tripId)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Trip WHERE IdTrip = @id";
            cmd.Parameters.AddWithValue("@id", tripId);
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        private async Task<bool> IsClientRegisteredForTrip(SqlConnection connection, int clientId, int tripId)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Client_Trip WHERE IdClient = @c AND IdTrip = @t";
            cmd.Parameters.AddWithValue("@c", clientId);
            cmd.Parameters.AddWithValue("@t", tripId);
            return (int)await cmd.ExecuteScalarAsync() > 0;
        }

        
        private async Task<(int current, int max)> GetTripCapacity(SqlConnection connection, int tripId)
        {
            int current;
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Client_Trip WHERE IdTrip = @t";
                cmd.Parameters.AddWithValue("@t", tripId);
                current = (int)await cmd.ExecuteScalarAsync();
            }

            int max;
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT MaxPeople FROM Trip WHERE IdTrip = @t";
                cmd.Parameters.AddWithValue("@t", tripId);
                max = (int)await cmd.ExecuteScalarAsync();
            }

            return (current, max);
        }

        public async Task<int> CreateClient(ClientCreateDTO client)
        {
            using var c = new SqlConnection(_conn);
            using var cmd = c.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
                VALUES (@fn, @ln, @em, @tel, @pesel);
                SELECT SCOPE_IDENTITY();";
            cmd.Parameters.AddWithValue("@fn", client.FirstName);
            cmd.Parameters.AddWithValue("@ln", client.LastName);
            cmd.Parameters.AddWithValue("@em", client.Email);
            cmd.Parameters.AddWithValue("@tel", 
               (object)client.Telephone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@pesel", client.Pesel);

            await c.OpenAsync();
            var id = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task<List<ClientTripDTO>> GetClientTrips(int clientId)
        {
            using var c = new SqlConnection(_conn);
            await c.OpenAsync();

            using var cmd = c.CreateCommand();
            cmd.CommandText = @"
                SELECT t.IdTrip, t.Name, t.Description, t.DateFrom, t.DateTo,
                       t.MaxPeople, ct.RegisteredAt, ct.PaymentDate
                FROM Client cl
                LEFT JOIN Client_Trip ct ON cl.IdClient = ct.IdClient
                LEFT JOIN Trip t         ON ct.IdTrip    = t.IdTrip
                WHERE cl.IdClient = @id";
            cmd.Parameters.AddWithValue("@id", clientId);

            using var r = await cmd.ExecuteReaderAsync();
            if (!r.HasRows)
            {
                await r.CloseAsync();
                bool exists = await ClientExists(c, clientId);
                return exists
                    ? new List<ClientTripDTO>()  
                    : null;                      
            }

            var list = new List<ClientTripDTO>();
            while (await r.ReadAsync())
            {
                if (r.IsDBNull(0)) continue; // skip rows with no trip
                list.Add(new ClientTripDTO {
                    TripId       = r.GetInt32(0),
                    TripName     = r.GetString(1),
                    Description  = r.IsDBNull(2) ? null : r.GetString(2),
                    DateFrom     = r.GetDateTime(3),
                    DateTo       = r.GetDateTime(4),
                    MaxPeople    = r.GetInt32(5),
                    RegisteredAt = r.GetInt32(6),
                    PaymentDate  = r.IsDBNull(7) ? (int?)null : r.GetInt32(7)
                });
            }
            return list;
        }

        public async Task<IClientsService.Result> RegisterClientToTrip(int clientId, int tripId)
        {
            using var c = new SqlConnection(_conn);
            await c.OpenAsync();

            if (!await ClientExists(c, clientId))
                return IClientsService.Result.ClientNotFound;

            if (!await TripExists(c, tripId))
                return IClientsService.Result.TripNotFound;

            var (current, max) = await GetTripCapacity(c, tripId);
            if (current >= max) return IClientsService.Result.Full;

            using var ins = c.CreateCommand();
            ins.CommandText = @"
                INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt)
                VALUES (@c, @t, CONVERT(int,FORMAT(GETDATE(),'yyyyMMdd')))";
            ins.Parameters.AddWithValue("@c", clientId);
            ins.Parameters.AddWithValue("@t", tripId);
            await ins.ExecuteNonQueryAsync();
            return IClientsService.Result.Success;
        }

        public async Task<IClientsService.Result> RemoveClientFromTrip(int clientId, int tripId)
        {
            using var c = new SqlConnection(_conn);
            await c.OpenAsync();

            if (!await ClientExists(c, clientId))
                return IClientsService.Result.ClientNotFound;

            if (!await TripExists(c, tripId))
                return IClientsService.Result.TripNotFound;

            if (!await IsClientRegisteredForTrip(c, clientId, tripId))
                return IClientsService.Result.NotRegistered;

            using var del = c.CreateCommand();
            del.CommandText = @"
                DELETE FROM Client_Trip 
                WHERE IdClient = @c AND IdTrip = @t";
            del.Parameters.AddWithValue("@c", clientId);
            del.Parameters.AddWithValue("@t", tripId);
            await del.ExecuteNonQueryAsync();
            return IClientsService.Result.Success;
        }
    }
}