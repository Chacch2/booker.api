using booker.api.Models;

namespace booker.api.Services.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
