using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Data;
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
}