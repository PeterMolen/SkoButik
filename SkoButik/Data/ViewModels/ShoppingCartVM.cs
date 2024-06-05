using SkoButik.Data.Cart;
using SkoButik.Models;

namespace SkoButik.Data.ViewModels
{
    public class ShoppingCartVM
    {
        public ShoppingCart? ShoppingCart { get; set; }
        public decimal ShoppingCartTotal { get; set; }
        public decimal AdjustedPrice { get; set; }
    }
}
