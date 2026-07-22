using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Data;
using api.Dtos.Stock;

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

    [HttpGet] // All Stocks
    public IActionResult GetAll()
    {
        var stocks = _context.Stocks
		 .Select(s => s.ToStockDto())
		 .ToList();
        
        return Ok(stocks);
    }

    [HttpGet("{id}")] // Stock by Id
    public IActionResult GetById([FromRoute] int id)
    {
        var stocks = _context.Stocks.Find(id);

        if (stocks == null)
        {
            return NotFound();
        }
        
        return Ok(stocks.ToStockDto());
    }

    [HttpPost] // Posting Stock
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();
        
        return CreatedAtAction(
            nameof(GetById), 
            new { id = stockModel.Id }, 
            stockModel.ToStockDto()
        );
    }
    
    [HttpPut("{id}")] // Updating Stock by Id
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);

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

        _context.SaveChanges();
        
        return Ok(stockModel.ToStockDto());
    }
}