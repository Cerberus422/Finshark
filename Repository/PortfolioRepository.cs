using api.Data;
using api.Dtos.Comment;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDBContext _context;
    
    public PortfolioRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<StockDto>> GetUserPortfolio(AppUser user)
    {
        return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new StockDto
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
                
            }).ToListAsync();
    }
    
    public async Task<List<StockWithCommentsDto>> GetUserPortfolioWithComments(AppUser user)
    {
        return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new StockWithCommentsDto
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
                
                Comments = stock.Stock.Comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Content = c.Content,
                    CreatedOn = c.CreatedOn
                
                }).ToList()
            
            }).ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await _context.AddAsync(portfolio);
        await _context.SaveChangesAsync();

        return portfolio;
    }

    public async Task<Portfolio> DeletePortfolio(AppUser user, string symbol)
    {
        var portfolioModel =
            await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol == symbol);

        if (portfolioModel == null)
        {
            return null;
        }

        _context.Portfolios.Remove(portfolioModel);

        await _context.SaveChangesAsync();

        return portfolioModel;
    }
}