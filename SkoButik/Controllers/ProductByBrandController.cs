using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkoButik.Data;
using SkoButik.Models;

namespace SkoButik.Controllers
{
    public class ProductByBrandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductByBrandController(ApplicationDbContext context)
        {
            _context = context;
        }
      
        public async Task<IActionResult> GetBrand(string brandName)
        {
            IQueryable<Product> productsByBrand = _context.Products;



                var productList = await productsByBrand
            .Include(p => p.Brand)
            .Where(p => p.Brand.BrandName == brandName) // Use the brandName parameter
            .Include(p => p.Inventories)
            .ThenInclude(i => i.Sizes)
            .ToListAsync();

        return View(productList);
        }
            
    }
}
