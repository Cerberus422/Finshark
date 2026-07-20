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

    [HttpGet]

    public IActionResult GetAll()
    {
        var stocks = _context.Stocks
		 .Select(s => s.ToStockDto())
		 .ToList();
        return Ok(stocks);
    }

    [HttpGet("{id}")]

    public IActionResult GetById([FromRoute] int id)
    {
        var stocks = _context.Stocks.Find(id);

        if (stocks == null)
        {
            return NotFound();
        }
        return Ok(stocks.ToStockDto());
    }

	[HttpGet("moreinfo")]

    public IActionResult GetAllInfo()
    {
        var stocks = _context.Stocks
 		 .Include(s => s.Comments)
		 .Select(s => s.ToStockWithCommentsDto())
		 .ToList();
        return Ok(stocks);
    }
	
	[HttpGet("moreinfo/{id}")]

    public IActionResult GetByIdAllInfo([FromRoute] int id)
    {
        var stocks = _context.Stocks
		 .Include(s => s.Comments)
		 .FirstOrDefault(s => s.Id == id);

        if (stocks == null)
        {
            return NotFound();
        }
        return Ok(stocks.ToStockWithCommentsDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }
}