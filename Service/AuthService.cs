using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Service;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager ;
    private readonly SignInManager<AppUser> _signInManager ;
    private readonly ITokenService _tokenService ;


    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task<NewUserDto?> RegisterAsync(RegisterDto registerDto)
    {
        var appUser = new AppUser
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };

        var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (createUser.Succeeded)
        {
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (roleResult.Succeeded)
            {
                return new NewUserDto
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    };
            }
        }

        return null;
    }

    public async Task<NewUserDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username.ToLower());

        if (user == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return null;
        
        return new NewUserDto
        {
            Username = user.UserName,
            Email  =  user.Email,
            Token = _tokenService.CreateToken(user)
        };
    }
}