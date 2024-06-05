using Microsoft.EntityFrameworkCore;
using SkoButik.Models;

namespace SkoButik.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ApplicationDbContext _context;
        public OrdersService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.ApplicationUser)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Products)
                .ThenInclude(oi => oi.Campaign)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Sizes)
                .ToListAsync();
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId)
        {
            var order = new Order()
            {
                UserId = userId
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {

                var product = await _context.Products
                                            .Include(p => p.Campaign)
                                            .FirstOrDefaultAsync(p => p.ProductId == item.Product.ProductId);

                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    FkProductId = item.Product.ProductId,
                    FkOrderId = order.OrderId,
                    Price = product.AdjustedPrice,
                    FkSizeId = item.Size.SizeId

                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();
        }
    }
    
}
