using Microsoft.EntityFrameworkCore;
using SkoButik.Models;

namespace SkoButik.Data.Cart
{
    public class ShoppingCart
    {
        public ApplicationDbContext _context { get; set; }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartitems { get; set; }

        public ShoppingCart(ApplicationDbContext context)
        {
            _context = context;
        }

        //get Shopping cart
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        //Add Item to Shopping cart
        public void AddItemToCart(Product product, int sizeId)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(
                n => n.FkProductId == product.ProductId &&
                n.FkSizeId == sizeId &&
                n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    FkProductId = product.ProductId,
                    FkSizeId = sizeId,
                    Amount = 1
                };

                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _context.SaveChanges();
        }

        //Remove Item from Shopping cart
        public void RemoveItemFromCart(Product product, int sizeId)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(
                n => n.FkProductId == product.ProductId &&
                n.FkSizeId == sizeId &&        // Kontrollera även storleken
                n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _context.SaveChanges();
        }

        //Get shopping cart items
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartitems ?? (ShoppingCartitems = _context.ShoppingCartItems
                .Where(n => n.ShoppingCartId == ShoppingCartId)
                .Include(n => n.Product)
                .Select(item => new ShoppingCartItem
                {
                    ShoppingCartItemId = item.ShoppingCartItemId,
                    FkProductId = item.FkProductId,
                    Product = item.Product,
                    FkSizeId = item.FkSizeId,
                    Size = _context.Inventories
                        .Where(i => i.FkProductId == item.FkProductId && i.FkSizeId == item.FkSizeId)
                        .Select(i => i.Sizes)
                        .FirstOrDefault(),
                    Amount = item.Amount
                })
                .ToList());
        }

        public decimal GetShoppingCartTotal()
        {
            var cartItems = _context.ShoppingCartItems
                .Where(n => n.ShoppingCartId == ShoppingCartId)
                .Include(n =>n.Product)
                .ThenInclude(n =>n.Campaign)
                .ToList();
            decimal total = 0;

            foreach(var cartItem in cartItems)
            {
                total += cartItem.Product.AdjustedPrice * cartItem.Amount;
            }
            return total;
        }


        //Clear Shopping Cart
        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId ==
            ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
