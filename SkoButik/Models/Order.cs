using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkoButik.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
