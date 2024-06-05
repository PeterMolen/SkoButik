using Microsoft.AspNetCore.Mvc;
using SkoButik.Data.Cart;

namespace SkoButik.Data.ViewComponents
{
    public class ShoppingCartSummary : ViewComponent
    {
        private readonly ShoppingCart _shoppingcart;
        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingcart = shoppingCart;
        }

        public IViewComponentResult Invoke()
        {
            var items = _shoppingcart.GetShoppingCartItems();

            return View(items.Count);
        }
    }
}
