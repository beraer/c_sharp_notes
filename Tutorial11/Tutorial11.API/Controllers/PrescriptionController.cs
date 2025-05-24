using Microsoft.AspNetCore.Mvc;
using Tutorial11.API.DTOs;
using Tutorial11.API.Services;

namespace Tutorial11.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionController(IPrescriptionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] NewPrescriptionDto dto)
    {
        try
        {
            await _service.AddPrescriptionAsync(dto);
            return Created("", "Prescription successfully added.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }
    
    [HttpGet("/api/patients/{id}")]
    public async Task<IActionResult> GetPatientWithPrescriptions(int id)
    {
        try
        {
            var result = await _service.GetPatientWithPrescriptionsAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

}