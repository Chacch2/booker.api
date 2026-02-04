using booker.api.Data.Configurations;
using booker.api.Models;
using Microsoft.EntityFrameworkCore;

namespace booker.api.Data;

public class BookerDbContext : DbContext
{
    public BookerDbContext(DbContextOptions<BookerDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookerDbContext).Assembly);
    }
}
