using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Tutorial9.Model;
using Tutorial9.Services;

namespace Tutorial9.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly DbService _dbService;

    public WarehouseController(DbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductWarehouseAsync(CreateProductWarehouseDTO request)
    {
        var res = await _dbService.CreateProductWarehouseAsync(request);
        return Ok(new {Id = res});
    }
    
    
    [HttpPost("warehouse")]
    public async Task<IActionResult> AddProductUsingProc([FromBody] CreateProductWarehouseDTO dto)
    {
        try
        {
            var newId = await _dbService.CreateProductWarehouseUsingProcAsync(dto);
            return Ok(new { Id = newId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(409, ex.Message); // Conflict or error from SP
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Unexpected error: " + ex.Message);
        }
    }


}