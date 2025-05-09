using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _svc;
        public TripsController(ITripsService svc) => _svc = svc;

        // GET /api/trips
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetTrips());

        // GET /api/trips/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var trip = await _svc.GetTripById(id);
            if (trip == null) return NotFound();
            return Ok(trip);
        }
    }
}