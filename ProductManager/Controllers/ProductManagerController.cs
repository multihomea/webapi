using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductManagerController : ControllerBase
    {
        private readonly ProductManagerContext _context;

        public ProductManagerController(ProductManagerContext context)
        {
            _context = context;
        }

          // GET: api/Product
        [HttpGet]
        public async  Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return  await _context.Products.ToListAsync();
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostProduct", new { id = product.ProductId }, product);
        }
    }
}
