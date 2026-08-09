using api.Dtos.Account;

namespace api.Interfaces;

public interface IAuthService
{
    Task<NewUserDto?> RegisterAsync(RegisterDto registerDto);
    Task<NewUserDto?> LoginAsync(LoginDto loginDto);
}