using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using booker.api.Models;

namespace booker.api.Data
{
    public class BookerIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookerIdentityDbContext(DbContextOptions<BookerIdentityDbContext> options) : base(options)
        {
        }
    }
}
