using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    
    // Get
    Task<List<Stock>> GetallStockAsync(); // All Stocks
    Task<List<Stock>> GetallStockWithCommentsAsync(); // All Stocks with Comments
    Task<Stock?> GetStockByIdAsync(int id); // Stock by Id
    Task<Stock?> GetStockWithCommentsByIdAsync(int id);  // Stock with Comments by Id
    
    // Post
    Task<Stock> CreateStockAsync(Stock stockModel); // Posting Stock
    Task<Comment> CreateCommentAsync(Comment commentModel); // Posting new Comment to existing Stock
    
    // Put
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto); // Updating Stock by Id
    
    // Delete
    Task<Stock?> DeleteAsync(int id); // Deleting whole Stock with all comments
    
    // Checking
    Task<bool> StockExists(int stockId); // Checking if Stock exists
}