using Microsoft.AspNetCore.Identity;

namespace api.Dtos.Account;

public class RegisterResult
{
    public NewUserDto? User { get; set; }
    public IEnumerable<IdentityError> Errors { get; set; } = [];
}