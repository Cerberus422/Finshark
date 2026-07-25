using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Data;
using api.Dtos.Stock;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    // Get
    [HttpGet] // All Stocks
    public async Task<IActionResult> GetAll()
    {
        var stocks = 
            (await _context.Stocks.ToListAsync()).
            Select(s => s.ToStockDto());
        
        
        return Ok(stocks);
    }

    [HttpGet("{id}")] // Stock by Id
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stocks = await _context.Stocks.FindAsync(id);

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
        
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = stockModel.Id }, 
            stockModel.ToStockDto()
        );
    }
    
    // Put
    [HttpPut("{id}")] // Updating Stock by Id
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap = updateDto.MarketCap;

        await _context.SaveChangesAsync();
        
        return Ok(stockModel.ToStockDto());
    }
    
    // Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id) // Deleting Stock by Id
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stockModel);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}