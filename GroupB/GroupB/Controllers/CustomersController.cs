using GroupB.Services;
using Microsoft.AspNetCore.Mvc;
using GroupB.DTOs;
using GroupB.Exceptions;

namespace GroupB.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IDbService _dbService;

    public CustomersController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}/purchases")]
    public async Task<IActionResult> GetPurchases(int id)
    {
        try
        {
            RespondCustomerPurchasesDto result = await _dbService.GetTicketsForCustomerAsync(id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCustomerWithPurchases([FromBody] RequestCustomerPurchaseDto dto)
    {
        try
        {
            await _dbService.AddCustomerWithPurchasesAsync(dto);
            return Created();
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

