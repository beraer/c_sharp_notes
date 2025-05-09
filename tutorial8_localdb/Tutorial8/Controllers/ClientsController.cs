using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _svc;
        public ClientsController(IClientsService svc) => _svc = svc;

        // GET /api/clients/{id}/trips
        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetTrips(int id)
        {
            var list = await _svc.GetClientTrips(id);
            if (list == null) return NotFound($"Client {id} not found");
            return Ok(list);
        }

        // POST /api/clients
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int newId = await _svc.CreateClient(dto);
            return CreatedAtAction(nameof(GetTrips),
                new { id = newId },
                new { id = newId });
        }

        // PUT /api/clients/{id}/trips/{tripId}
        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> Register(int id, int tripId)
        {
            var res = await _svc.RegisterClientToTrip(id, tripId);
            return res switch
            {
                IClientsService.Result.ClientNotFound
                    => NotFound($"Client {id} not found"),
                IClientsService.Result.TripNotFound
                    => NotFound($"Trip {tripId} not found"),
                IClientsService.Result.Full
                    => BadRequest("Trip is full"),
                _ => Ok()
            };
        }

        // DELETE /api/clients/{id}/trips/{tripId}
        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> Remove(int id, int tripId)
        {
            var res = await _svc.RemoveClientFromTrip(id, tripId);
            return res switch
            {
                IClientsService.Result.ClientNotFound
                    => NotFound($"Client {id} not found"),
                IClientsService.Result.TripNotFound
                    => NotFound($"Trip {tripId} not found"),
                IClientsService.Result.NotRegistered
                    => BadRequest("Client is not registered for this trip"),
                _ => Ok()
            };
        }
    }
}
