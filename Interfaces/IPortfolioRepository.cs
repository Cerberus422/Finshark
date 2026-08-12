using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    Task<List<StockDto>> GetUserPortfolio(AppUser user);
    Task<List<StockWithCommentsDto>> GetUserPortfolioWithComments(AppUser user);
    Task<Portfolio> CreateAsync(Portfolio portfolio);
    Task<Portfolio> DeletePortfolio(AppUser user, string symbol);
}