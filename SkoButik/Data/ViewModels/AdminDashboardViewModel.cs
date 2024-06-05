using SkoButik.Models;

namespace SkoButik.Data.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<OrderStatsViewModel> OrderStats { get; set; }
        public ProductStatsViewModel MostSoldProduct { get; set; }
        public List<Order> Orders { get; set; }
        public List<Inventory> Inventories { get; set; }
    }
}
