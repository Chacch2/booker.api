using Microsoft.AspNetCore.Identity;

namespace booker.api.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
