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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // 進一步的模型配置（如果需要）

            var adminId = "93cf9253-0248-423b-8f99-b140e49383b2";
            var userId = "c6347742-7b4a-4417-b440-345ac8232796";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminId
                },
                new IdentityRole
                {
                    Id = userId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userId

                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
