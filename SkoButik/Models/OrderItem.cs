using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkoButik.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [ForeignKey("Product")]
        public int FkProductId { get; set; }
        public Product? Products { get; set; }

        [ForeignKey("Order")]
        public int FkOrderId { get; set; }
        public Order? Orders { get; set; }

        [ForeignKey("Size")]
        public int FkSizeId { get; set; }
        public Size? Sizes { get; set; }
    }
}
