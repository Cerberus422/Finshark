using api.Data;
using api.Dtos.Stock;
using api.Helpers;
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
        
    public async Task<List<Stock>> GetallStockAsync(QueryObject query)
    {
        var stock = _context.Stocks
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stock = stock.Where(s => s.Symbol.Contains(query.Symbol));
        }
        
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stock = stock.Where(s => s.CompanyName.Contains(query.CompanyName));
        }
        
        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(s => s.Symbol);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<List<Stock>> GetallStockWithCommentsAsync(QueryObject query)
    {
        var stock = _context.Stocks
            .Include(s => s.Comments)
            .ThenInclude(a => a.AppUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stock = stock.Where(s => s.Symbol.Contains(query.Symbol));
        }
        
        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stock = stock.Where(s => s.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(s => s.Symbol);
            }
        }
        
        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        
    }

    public async Task<Stock?> GetStockByIdAsync(int id)
    {
       return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock?> GetStockBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock?> GetStockWithCommentsByIdAsync(int id)
    {
       return await _context.Stocks
           .Include(s => s.Comments)
           .ThenInclude(a => a.AppUser)
           .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock> CreateStockAsync(Stock stockModel)
    {
        await  _context.Stocks.AddAsync(stockModel);
        await  _context.SaveChangesAsync();
        
        return stockModel;
    }
    

    public async Task<Stock?> UpdateAsync(int id, Stock stockDto)
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

    public  async Task<bool> StockExists(int stockId)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == stockId);
    }
}