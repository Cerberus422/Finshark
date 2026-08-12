using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
    
public class PortfolioControler : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfoliorepo;
    
    public PortfolioControler(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfoliorepo)
    {
        _userManager = userManager;
        _stockRepo = stockRepo;
        _portfoliorepo = portfoliorepo;
    }


    [HttpGet]
    [Authorize]

    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfoliorepo.GetUserPortfolio(appUser);
        
        return Ok(userPortfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepo.GetBySymbolAsync(symbol);

        if (stock == null) 
            return BadRequest("Stock not found");

        var userPortfolio = await _portfoliorepo.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Cannot Add same stock to portfolio");

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };

        await _portfoliorepo.CreateAsync(portfolioModel);

        if (portfolioModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else
        {
            return Created();
        }
    }
    
    [HttpDelete]
    [Authorize]

    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        
        var userPortfolio = await _portfoliorepo.GetUserPortfolio(appUser);

        var filterStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filterStock.Count() == 1)
        {
            await _portfoliorepo.DeletePortfolio(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not in your Portfolio");
        }
        
        return Ok();
    }
}