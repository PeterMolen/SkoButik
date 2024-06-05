using SkoButik.Models;

namespace SkoButik.Data.Services
{
    public interface IOrdersService
    {
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);

    }
}
