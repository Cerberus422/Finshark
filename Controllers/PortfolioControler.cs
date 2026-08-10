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
}