using Microsoft.AspNetCore.Mvc;
using SkoButik.Data.Cart;
using SkoButik.Data.Services;
using SkoButik.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using SkoButik.Data.ViewModels;
using SkoButik.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SkoButik.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoppingCart _shoppingCart;
        private readonly ApplicationDbContext _context;
        private readonly IOrdersService _ordersService;

        private static readonly string _exchangeRateApiKey = "6e9a8cd53905eb9043c5f0ec";
        private static readonly string _googleApiKey = "AIzaSyAPfoMTCytxHoeQKoactrGV6sJyKDRfSPQ";
        private static readonly string _origin = "Håstaholmen 4, 824 44 Hudiksvall";

        public OrdersController(ShoppingCart shoppingCart, ApplicationDbContext context, IOrdersService ordersService)
        {
            _shoppingCart = shoppingCart;
            _context = context;
            _ordersService = ordersService;
        }


        //SearchFilter
        public async Task<IActionResult> Filter(string searchString)
        {
            var allProducts = await _context.Products.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allProducts.Where(n =>
                    n.ProductName.ToLower().Contains(searchString.ToLower()) ||
                    n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                return View("Index", filteredResult);
            }

            return View("Index", allProducts);
        }

        ///________________________________________________________________________________

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _ordersService.GetOrdersByUserIdAsync(userId);

            foreach (var order in orders)
            {
                Console.WriteLine($"OrderID: {order.OrderId}, UserID: {order.UserId}, FirstName: {order.ApplicationUser?.FirstName}");
                Console.WriteLine($"OrderItems Count: {order.OrderItems.Count}");
                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"ProductName: {item.Products?.ProductName}, Price: {item.Products?.AdjustedPrice}, Amount: {item.Amount}");
                }
            }

            return View(orders);
        }
    



    //__________________________________________________________________________


    // Shopping Cart
    public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartitems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }

        //Add item to shopping cart
        public async Task<IActionResult> AddItemToShoppingCart(int id, int sizeId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);

            if (product != null)
            {
                _shoppingCart.AddItemToCart(product, sizeId);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        //Remove item from shopping cart
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id, int sizeId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);

            if (product != null)
            {
                _shoppingCart.RemoveItemFromCart(product, sizeId);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        // Check inventory status
        [HttpGet]
        public async Task<IActionResult> CheckInventoryStatus(int productId, int sizeId)
        {
            var inventoryItem = await _context.Inventories.FirstOrDefaultAsync(i => i.FkProductId == productId && i.FkSizeId == sizeId);
            if (inventoryItem != null && inventoryItem.Quantity > 0)
            {
                return Ok(new { status = "available", quantity = inventoryItem.Quantity });
            }
            else
            {
                return Ok(new { status = "out_of_stock", quantity = 0 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckInventoryBeforeAddingToCart(int productId, int sizeId)
        {
            var cartItemCountForSize = _shoppingCart.GetShoppingCartItems().Where(item => item.FkProductId == productId && item.FkSizeId == sizeId).Sum(item => item.Amount);
            var inventoryItem = await _context.Inventories.FirstOrDefaultAsync(i => i.FkProductId == productId && i.FkSizeId == sizeId);

            if (inventoryItem != null && inventoryItem.Quantity - cartItemCountForSize > 0)
            {
                return Ok(new { canAddToCart = true });
            }
            else
            {
                return Ok(new { canAddToCart = false });
            }
        }

        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            var cartItemCount = _shoppingCart.GetShoppingCartItems().Sum(item => item.Amount);
            return Ok(new { cartItemCount });
        }


        // Complete order
        public async Task<IActionResult> CompleteOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("NotLoggedIn");
            }

            var items = _shoppingCart.GetShoppingCartItems().ToList();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var item in items)
            {
                var inventoryItem = await _context.Inventories.FirstOrDefaultAsync(i => i.FkProductId == item.Product.ProductId && i.FkSizeId == item.Size.SizeId);

                if (inventoryItem == null || inventoryItem.Quantity < item.Amount)
                {
                    ModelState.AddModelError("", $"Not enough stock for product {item.Product.ProductName} in size {item.Size?.SizeName}");
                    return View("ShoppingCart", new ShoppingCartVM { ShoppingCart = _shoppingCart, ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal() });
                }
            }

            await _ordersService.StoreOrderAsync(items, userId);


            foreach (var item in items)
            {
                var inventoryItem = await _context.Inventories.FirstOrDefaultAsync(i => i.FkProductId == item.Product.ProductId && i.FkSizeId == item.Size.SizeId);
                inventoryItem.Quantity -= item.Amount;
            }
            await _context.SaveChangesAsync();

            await _shoppingCart.ClearShoppingCartAsync();
            return View("OrderCompleted");
        }

        // Action for NotLoggedIn view
        public IActionResult NotLoggedIn()
        {
            return View();
        }

        //_______________________________________________________________________

        // OrderStats

        //public IActionResult OrderStats()
        //{
        //    var orderItems = _context.Orders
        //        .Include(o => o.OrderItems)
        //        .ThenInclude(o => o.Products)
        //        .Where(o => o.OrderDate.Date >= DateTime.UtcNow.Date.AddDays(-7))
        //        .SelectMany(o => o.OrderItems)
        //        .ToList();

        //    // Create an instance of the adapter
        //    var adapter = new OrderStatsAdapter(_context);

        //    var orderStats = orderItems
        //        .GroupBy(o => o.Orders?.OrderDate.Date)
        //        .Select(g => new 
        //        {
        //            OrderDate = g.Key,
        //            OrderCount = g.Count(),
        //            TotalSales = adapter.CalculateTotalSales(g.ToList())
        //        })
        //        .OrderByDescending(o => o.OrderDate)
        //        .ToList()
        //     .Select(o => new OrderStatsViewModel
        //      {
        //          OrderDate = (DateTime)o.OrderDate,
        //          OrderCount = o.OrderCount,
        //          TotalSales = o.TotalSales
        //      })
        //        .ToList();

        //    return View(orderStats);
        //}

        public IActionResult OrderStats()
        {
                // Create an instance of the adapter
                var adapter = new OrderStatsAdapter(_context);

            var orderStats = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Products)
                .ThenInclude(o => o.Campaign)
                .Where(o => o.OrderDate.Date >= DateTime.UtcNow.Date.AddDays(-7)) // Only consider orders from the last 7 days
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

            return View(orderStats);
        }

        // MostSold
        public IActionResult MostSold()
        {
            var productStats = _context.OrderItems
                .Include(oi => oi.Products)
                .Where(oi => oi.Orders.OrderDate.Date >= DateTime.UtcNow.Date.AddDays(-7)) // Only consider orders from the last 7 days
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

            return View(mostSoldProduct);
        }

        public async Task<IActionResult> OrderList(string currency = "SEK")
        {
            var orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(o => o.Products).ThenInclude(o => o.Campaign).ToListAsync();
            decimal exchangeRate = 1.0m;

            if (currency != "SEK")
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync($"https://v6.exchangerate-api.com/v6/{_exchangeRateApiKey}/latest/SEK");
                    var rates = JObject.Parse(response)["conversion_rates"];
                    exchangeRate = rates[currency].Value<decimal>();
                }
            }

            var model = new OrderListViewModel
            {
                Orders = orders,
                SelectedCurrency = currency,
                ExchangeRate = exchangeRate
            };

            return View(model);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Products)
                    .ThenInclude(oi => oi.Campaign)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Products)
                        .ThenInclude(p => p.Inventories)
                            .ThenInclude(i => i.Sizes)
                .Include(o => o.ApplicationUser)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var totalPrice = order.OrderItems.Sum(oi => oi.Amount * oi.Products.AdjustedPrice);

            var destination = $"{order.ApplicationUser.Address}, {order.ApplicationUser.ZipCode} {order.ApplicationUser.City}";

            var distance = await GetDistanceAsync(_origin, destination);

            var model = new OrderDetailsViewModel
            {
                Order = order,
                OrderedBy = order.ApplicationUser,
                TotalPrice = totalPrice,
                Distance = distance
            };

            return View(model);
        }

        // Calculate distance between warehouse and customer
        private static async Task<string> GetDistanceAsync(string origin, string destination)
        {
            using (var client = new HttpClient())
            {
                var originEncoded = Uri.EscapeDataString(origin);
                var destinationEncoded = Uri.EscapeDataString(destination);

                var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={originEncoded}&destinations={destinationEncoded}&key={_googleApiKey}";
                var response = await client.GetStringAsync(url);
                var data = JObject.Parse(response);

                var element = data["rows"]?.FirstOrDefault()?["elements"]?.FirstOrDefault();
                if (element?["status"]?.ToString() == "OK")
                {
                    var distance = element["distance"]?["text"]?.ToString();
                    var duration = element["duration"]?["text"]?.ToString();
                    return $"Distance: {distance}, Duration: {duration}";
                }
                return "Unable to calculate distance.";
            }
        }

    }
}
