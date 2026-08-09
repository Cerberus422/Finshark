using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IAuthService authService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _authService.RegisterAsync(registerDto);

        if (user == null)
        {
            return BadRequest("Unable to create user.");
        }
        
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
        {
            return Unauthorized("Invalid Username or Password");
        }

        return Ok(result);
    }
}

