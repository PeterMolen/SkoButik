using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkoButik.Data;
using SkoButik.Data.ViewModels;
using SkoButik.Models;
using SkoButik.Utility;

namespace SkoButik.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashBoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DashBoard()
        {
            // Retrieve all order items
            List<OrderItem> getAll = await _context.OrderItems
                .Include(c => c.Products)
                .ToListAsync();

            // Calculate the total income
            decimal CountIncome = getAll.Sum(j => j.Amount * j.Products.AdjustedPrice);

            ViewBag.CountIncome = CountIncome.ToString("C0");

            // Calculate today's income
            DateTime today = DateTime.UtcNow.Date;
            var todayOrderItems = await _context.OrderItems
                .Where(j => j.Orders.OrderDate.Date == today)
                .ToListAsync();

            decimal TodayIncome = todayOrderItems.Sum(j => j.Amount * j.Products.AdjustedPrice);

            ViewBag.TodayIncome = TodayIncome.ToString("C0");


            // Calculate sales data for the last 7 days
            DateTime sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
            

            var purchasesData = await _context.OrderItems
                .Where(oi => oi.Orders.OrderDate.Date >= sevenDaysAgo && oi.Orders.OrderDate.Date <= today)
                .GroupBy(oi => new { Date = oi.Orders.OrderDate.Date, oi.FkProductId })
                .Select(g => new
                {
                    Date = g.Key.Date,
                    ProductId = g.Key.FkProductId,
                    TotalAmount = g.Sum(oi => oi.Amount)
                })
                .OrderBy(d => d.Date)
                .ToListAsync();

            var groupedData = purchasesData
                .GroupBy(pd => pd.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Products = g.ToDictionary(x => x.ProductId, x => x.TotalAmount)
                })
                .ToList();

            var dates = Enumerable.Range(0, 7)
                .Select(offset => sevenDaysAgo.AddDays(offset).ToString("yyyy-MM-dd"))
                .ToList();

            var productIds = purchasesData.Select(d => d.ProductId).Distinct().ToList();

            var productAmounts = productIds.ToDictionary(
                productId => productId,
                productId => dates.Select(date =>
                {
                    var dateData = groupedData.FirstOrDefault(g => g.Date.ToString("yyyy-MM-dd") == date);
                    return dateData != null && dateData.Products.ContainsKey(productId) ? dateData.Products[productId] : 0;
                }).ToList()
            );

            ViewBag.Dates = dates;
            ViewBag.ProductAmounts = productAmounts;

            //get campign table
            List<Campaign> getCampaign = await _context.Campaigns
                .ToListAsync();
            ViewBag.Campaigns = getCampaign;

            // Order statistics for the last 7 days
            var orderStats = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Products)
                .ThenInclude(o => o.Campaign)
                .Where(o => o.OrderDate.Date >= sevenDaysAgo)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    OrderDate = g.Key,
                    OrderCount = g.Count(),
                    TotalSales = g.SelectMany(o => o.OrderItems).Sum(oi => oi.Amount * oi.Price)
                })
                .OrderByDescending(o => o.OrderDate)
                .ToList()
                .Select(o => new OrderStatsViewModel
                {
                    OrderDate = o.OrderDate,
                    OrderCount = o.OrderCount,
                    TotalSales = o.TotalSales
                })
                .ToList();

            // Most sold product in the last 7 days
            var productStats = _context.OrderItems
                .Include(oi => oi.Products)
                .Where(oi => oi.Orders.OrderDate.Date >= sevenDaysAgo)
                .GroupBy(oi => oi.FkProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.FirstOrDefault().Products.ProductName,
                    TotalQuantity = g.Sum(oi => oi.Amount),
                    ImageUrl = g.FirstOrDefault().Products.ImageUrl
                })
                .OrderByDescending(p => p.TotalQuantity)
                .FirstOrDefault();

            var mostSoldProduct = productStats != null ? new ProductStatsViewModel
            {
                ProductName = productStats.ProductName,
                TotalQuantity = productStats.TotalQuantity,
                ImageUrl = productStats.ImageUrl,
            } : null;

            // Recent orders
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Products)
                .ThenInclude(o => o.Campaign)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Inventory count grouped by brand
            var brandInventory = await _context.Inventories
                .GroupBy(i => i.Products.Brand.BrandName)
                .Select(g => new
                {
                    Brand = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                })
                .ToListAsync();

            var brands = brandInventory.Select(b => b.Brand).ToList();
            var quantities = brandInventory.Select(b => b.Quantity).ToList();

            ViewBag.Brands = brands;
            ViewBag.Quantities = quantities;

            var model = new AdminDashboardViewModel
            {
                OrderStats = orderStats,
                MostSoldProduct = mostSoldProduct,
                Orders = orders,
                Inventories = await _context.Inventories.ToListAsync()
            };

            return View(model);
        }

    }

}
