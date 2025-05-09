using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services
{
    public interface IClientsService
    {
        Task<List<ClientTripDTO>> GetClientTrips(int clientId);
        Task<int> CreateClient(ClientCreateDTO client);
        Task<Result> RegisterClientToTrip(int clientId, int tripId);
        Task<Result> RemoveClientFromTrip(int clientId, int tripId);

        public enum Result
        {
            Success,
            ClientNotFound,
            TripNotFound,
            Full,
            NotRegistered
        }
    }
}