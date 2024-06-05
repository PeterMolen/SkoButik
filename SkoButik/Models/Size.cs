using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkoButik.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 10)]
        [DisplayName("Size")]
        public string SizeName {  get; set; }

        public ICollection<Inventory>? Inventories { get; set; }
        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
