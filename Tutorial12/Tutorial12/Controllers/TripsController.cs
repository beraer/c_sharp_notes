using Microsoft.AspNetCore.Mvc;
using Tutorial12.DTOs;
using Tutorial12.Services;

namespace Tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, [FromBody] AddClientTripDTO dto)
    {
        try
        {
            await _tripService.AddClientToTripAsync(idTrip, dto);
            return Ok(new { message = "Client added to trip." });
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("already exists") || ex.Message.Contains("already registered") || ex.Message.Contains("already started"))
                return BadRequest(ex.Message);
            if (ex.Message.Contains("not found"))
                return NotFound(ex.Message);
            return BadRequest(ex.Message);
        }
    }
} 