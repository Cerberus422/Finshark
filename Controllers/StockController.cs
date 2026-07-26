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

    public async Task<IActionResult> GetAllStocks()
    {
        var stocks =  await _context.Stocks.ToListAsync();
		 
        var stockDto = stocks.Select(s => s.ToStockDto());
        
        return Ok(stockDto);
    }
    
    [HttpGet("stocks-with-comments")]   // All Stocks with Comments

    public async Task<IActionResult> GetAllStocksWithComments()
    {
        var stocks = await _context.Stocks
            .Include(s => s.Comments).ToListAsync();

        var stockDtos = stocks.Select(s => s.ToStockWithCommentsDto());
            
        
        return Ok(stockDtos);
    }
    
    [HttpGet("{id}")]   // Stock by Id

    public async Task<IActionResult> GetStockById([FromRoute] int id)
    {
        var stocks = await _context.Stocks.FindAsync(id);

        if (stocks == null)
        {
            return NotFound();
        }
        
        return Ok(stocks.ToStockDto());
    }

	[HttpGet("stock-with-comments/{id}")]  // Stocks with Comments by Id

    public async Task<IActionResult> GetByIdStocksAndComments([FromRoute] int id)
    {
        var stocks = await _context.Stocks
		 .Include(s => s.Comments)
		 .FirstOrDefaultAsync(s => s.Id == id);

        if (stocks == null)
        {
            return NotFound();
        }
        
        return Ok(stocks.ToStockWithCommentsDto());
    }

    
    // Post
    [HttpPost]  // Posting Stock
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        
        await  _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetStockById), 
            new { id = stockModel.Id }, 
            stockModel.ToStockDto()
            );
    }
    
    [HttpPost("write-stock-and-comment")]  // Posting Stock with Comment
    public async Task<IActionResult> CreateStockWithComments([FromBody] CreateStockRequestWithCommentsDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetByIdStocksAndComments), 
            new { id = stockModel.Id }, 
            stockModel.ToStockWithCommentsDto()
        );
    }
    
    [HttpPost("write-comments/{id}")] // Posting new Comment to existing Stock
    public async Task<IActionResult> CreateComment([FromRoute] int id, [FromBody] CreateCommentRequestDto commentDto)
    {
        var stockModel = await _context.Stocks
            .Include(s => s.Comments)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        var comment = commentDto.ToCommentFromCreateDto();

        comment.StockId  = id;
        
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return Ok(stockModel.ToStockWithCommentsDto());
    }
    
    // Update
    [HttpPut("update-stocks/{id}")] // Updating Stock by Id

    public async Task<IActionResult> UpdateStocks([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = await _context.Stocks
            .FirstOrDefaultAsync(s => s.Id == id);

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

    [HttpPut("update-stocks-and-comments/{id}")] // Updating Stock and Comments by Id

    public async Task<IActionResult> UpdateStocksWithComments([FromRoute] int id, [FromBody] UpdateStockWithCommentsRequestDto updateDto)
    {
        var stockModel = await _context.Stocks
            .Include(s => s.Comments)
            .FirstOrDefaultAsync(s => s.Id == id);

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
        
        await _context.SaveChangesAsync();
        
        return Ok(stockModel.ToStockWithCommentsDto());
    }
    
    // Delete
    [HttpDelete("{id}")] 
    public async Task<IActionResult> Delete([FromRoute] int id) // Deleting whole Stock with all comments
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