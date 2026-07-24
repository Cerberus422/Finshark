using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Data;
using api.Dtos.Stock;
using api.Dtos.Comment;
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
    [HttpGet]   // All Stocks

    public IActionResult GetAllStocks()
    {
        var stocks = _context.Stocks
		 .Select(s => s.ToStockDto())
		 .ToList();
        
        return Ok(stocks);
    }
    
    [HttpGet("stocks-with-comments")]   // All Stocks with Comments

    public IActionResult GetAllStocksWithComments()
    {
        var stocks = _context.Stocks
            .Include(s => s.Comments)
            .Select(s => s.ToStockWithCommentsDto())
            .ToList();
        
        return Ok(stocks);
    }
    
    [HttpGet("{id}")]   // Stock by Id

    public IActionResult GetStockById([FromRoute] int id)
    {
        var stocks = _context.Stocks.Find(id);

        if (stocks == null)
        {
            return NotFound();
        }
        
        return Ok(stocks.ToStockDto());
    }

	[HttpGet("stock-with-comments/{id}")]  // Stocks with Comments by Id

    public IActionResult GetByIdStocksAndComments([FromRoute] int id)
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

    
    // Post
    [HttpPost]  // Posting Stock
    public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();

        return CreatedAtAction(
            nameof(GetStockById), 
            new { id = stockModel.Id }, 
            stockModel.ToStockDto()
            );
    }
    
    [HttpPost("write-stock-and-comment")]  // Posting Stock with Comment
    public IActionResult CreateStockWithComments([FromBody] CreateStockRequestWithCommentsDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        
        _context.Stocks.Add(stockModel);
        _context.SaveChanges();

        return CreatedAtAction(
            nameof(GetByIdStocksAndComments), 
            new { id = stockModel.Id }, 
            stockModel.ToStockWithCommentsDto()
        );
    }
    
    [HttpPost("write-comments/{id}")] // Posting new Comment to existing Stock
    public IActionResult CreateComment([FromRoute] int id, [FromBody] CreateCommentRequestDto commentDto)
    {
        var stockModel = _context.Stocks
            .Include(s => s.Comments)
            .FirstOrDefault(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        var comment = commentDto.ToCommentFromCreateDto();

        comment.StockId  = id;
        
        _context.Comments.Add(comment);
        _context.SaveChanges();

        return Ok(stockModel.ToStockWithCommentsDto());
    }
    
    // Update
    [HttpPut("update-stocks/{id}")] // Updating Stock by Id

    public IActionResult UpdateStocks([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = _context.Stocks
            .FirstOrDefault(s => s.Id == id);

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

    [HttpPut("update-stocks-and-comments/{id}")] // Updating Stock and Comments by Id

    public IActionResult UpdateStocksWithComments([FromRoute] int id, [FromBody] UpdateStockWithCommentsRequestDto updateDto)
    {
        var stockModel = _context.Stocks
            .Include(s => s.Comments)
            .FirstOrDefault(s => s.Id == id);

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
        
       foreach(var commentDto in updateDto.Comments) 
       {
           var comment = stockModel.Comments.FirstOrDefault(c => c.Id == commentDto.Id);

           if (comment == null)
           {
               continue;
           }
           
           comment.Title = commentDto.Title;
           comment.Content = commentDto.Content;
           comment.CreatedOn = commentDto.CreatedOn;
       }
        
        _context.SaveChanges();
        
        return Ok(stockModel.ToStockWithCommentsDto());
    }
    
    // Delete
    [HttpDelete("{id}")] // Deleting whole Stock with all comments
    public IActionResult Delete([FromRoute] int id) // Deleting Stock by Id
    {
        var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stockModel);

        _context.SaveChanges();

        return NoContent();
    }
}