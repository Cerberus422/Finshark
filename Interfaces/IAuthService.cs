using api.Dtos.Account;

namespace api.Interfaces;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterDto registerDto);
    Task<NewUserDto?> LoginAsync(LoginDto loginDto);
}