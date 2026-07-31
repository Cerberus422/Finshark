using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepo;
    public StockController(IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    // Get
    [HttpGet] // All Stocks
    public async Task<IActionResult> GetAll()
    {
        var stocks =
            (await _stockRepo.GetallAsync()).Select(s => s.ToStockDto());
        
        
        return Ok(stocks);
    }

    [HttpGet("{id}")] // Stock by Id
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stocks = await _stockRepo.GetByIdAsync(id);
 
        if (stocks == null)
        {
            return NotFound();
        }
        
        return Ok(stocks.ToStockDto());
    }

    // Post
    [HttpPost] // Posting Stock
    public async  Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();

        await _stockRepo.CreateAsync(stockModel);
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = stockModel.Id }, 
            stockModel.ToStockDto()
        );
    }
    
    // Update
    [HttpPut("{id}")] // Updating Stock by Id
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = await _stockRepo.UpdateAsync(id, updateDto.ToStockFromUpdateDto());

        if (stockModel == null)
        {
            return NotFound();
        }
        
        return Ok(stockModel.ToStockDto());
    }
    
    // Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) // Deleting Stock by Id
    {
        var stockModel = await _stockRepo.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}