using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _context;

    public StockRepository(ApplicationDBContext context)
    {
        _context = context;
    }
        
    public async Task<List<Stock>> GetallStockAsync()
    {
        return await _context.Stocks.ToListAsync();
    }

    public async Task<List<Stock>> GetallStockWithCommentsAsync()
    {
        return await _context.Stocks
            .Include(s => s.Comments).ToListAsync();
    }

    public async Task<Stock?> GetStockByIdAsync(int id)
    {
       return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock?> GetStockWithCommentsByIdAsync(int id)
    {
       return await _context.Stocks
           .Include(s => s.Comments)
           .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock> CreateStockAsync(Stock stockModel)
    {
        await  _context.Stocks.AddAsync(stockModel);
        await  _context.SaveChangesAsync();
        
        return stockModel;
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
        var stockModel = await _context.Stocks
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return null;
        }
        
        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName = stockDto.CompanyName;
        stockModel.Purchase = stockDto.Purchase;
        stockModel.LastDiv = stockDto.LastDiv;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return null;
        }

        _context.Stocks.Remove(stockModel);

        await _context.SaveChangesAsync();

        return stockModel;
    }
}