using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Data;
using api.Dtos.Stock;
using api.Dtos.Comment;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
   
    private readonly IStockRepository _stockRepo;
    public StockController( IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    // Get
    [HttpGet]   // All Stocks

    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = 
            (await _stockRepo.GetallStockAsync())
            .Select(s => s.ToStockDto());
        
        return Ok(stocks);
    }
    
    [HttpGet("stocks-with-comments")]   // All Stocks with Comments

    public async Task<IActionResult> GetAllStocksWithComments()
    {
        var stocks = 
            (await _stockRepo.GetallStockWithCommentsAsync())
            .Select(s => s.ToStockWithCommentsDto());

       return Ok(stocks);
    }
    
    [HttpGet("{id}")]   // Stock by Id

    public async Task<IActionResult> GetStockById([FromRoute] int id)
    {
        var stocks = await _stockRepo.GetStockByIdAsync(id);

        if (stocks == null)
        {
            return NotFound();
        }
        
        return Ok(stocks.ToStockDto());
    }

	[HttpGet("stock-with-comments/{id}")]  // Stock with Comments by Id

    public async Task<IActionResult> GetByIdStockAndComments([FromRoute] int id)
    {
        var stocks = await _stockRepo.GetStockWithCommentsByIdAsync(id);

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

        await _stockRepo.CreateStockAsync(stockModel);

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

        await _stockRepo.CreateStockAsync(stockModel);

        return CreatedAtAction(
            nameof(GetByIdStockAndComments), 
            new { id = stockModel.Id }, 
            stockModel.ToStockWithCommentsDto()
        );
    }
    
    // Update
    [HttpPut("update-stocks/{id}")] // Updating Stock by Id

    public async Task<IActionResult> UpdateStocks([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
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
    public async Task<IActionResult> Delete([FromRoute] int id) // Deleting whole Stock with all comments
    {
        var stockModel = await _stockRepo.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}