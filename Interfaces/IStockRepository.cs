using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    
    // Get
    Task<List<Stock>> GetallStockAsync(QueryObject query); // All Stocks
    Task<List<Stock>> GetallStockWithCommentsAsync(QueryObject query); // All Stocks with Comments
    Task<Stock?> GetStockByIdAsync(int id); // Stock by Id
    Task<Stock?> GetStockWithCommentsByIdAsync(int id);  // Stock with Comments by Id
    
    // Post
    Task<Stock> CreateStockAsync(Stock stockModel); // Posting Stock
    
    // Put
    Task<Stock?> UpdateAsync(int id, Stock stockDto); // Updating Stock by Id
    
    // Delete
    Task<Stock?> DeleteAsync(int id); // Deleting whole Stock with all comments
    
    // Checking
    Task<bool> StockExists(int stockId); // Checking if Stock exists
}