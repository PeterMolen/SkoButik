using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkoButik.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [ForeignKey("Product")]
        public int FkProductId { get; set; }
        public Product? Products { get; set; }

        [ForeignKey("Size")]
        public int FkSizeId { get; set; }
        public Size? Sizes { get; set; }

        public int Quantity { get; set; }
    }

}
