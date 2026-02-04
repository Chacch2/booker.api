using booker.api.Data;
using booker.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace booker.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly BookerDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public ProductController(BookerDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    // GET: api/Product
    [HttpGet("GetProducts")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        List<Product>? products = _memoryCache.Get<List<Product>>("products_cache");

        if (products == null)
        {
            products = await _context.Products.ToListAsync();

            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            };
            _memoryCache.Set("products_cache", products, options);
        }
        return products;
    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    // POST: api/Product
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _memoryCache.Remove("products_cache");

        return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    // PUT: api/Product/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Product/5/purchase
    [HttpPost("{id}/purchase")]
    public async Task<IActionResult> PurchaseProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        if (product.Stock <= 0)
        {
            return BadRequest("Out of stock");
        }

        product.Stock--;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Purchase successful", remainingStock = product.Stock });
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
