namespace SkoButik.Models
{
    public class OrderListViewModel
    {
        public List<Order> Orders { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
    }
}