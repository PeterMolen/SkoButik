using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkoButik.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Brandname must be between 2 and 20 characters")]
        [DisplayName("Brand")]
        public string BrandName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
