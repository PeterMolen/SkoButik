using SkoButik.Models;

namespace SkoButik.Data.Services
{
    public interface ITarget
    {
        decimal CalculateTotalSales(List<OrderItem> orderItems);
    }
}
