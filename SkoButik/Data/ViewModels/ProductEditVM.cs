using Microsoft.AspNetCore.Mvc.Rendering;
using SkoButik.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkoButik.Data.ViewModels
{
    public class ProductEditVM
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Productname must be between 2 and 50 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [StringLength(60, MinimumLength = 10)]
        public string? Description { get; set; }

        [StringLength(60, MinimumLength = 20)]
        public string? ImageUrl { get; set; } //ändra till Byte[]???

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public List<int> SelectedBrand { get; set; }
        public IEnumerable<SelectListItem> Brands { get; set; }
        public List<int> SelectedSize { get; set; }
        public IEnumerable<SelectListItem> Sizes { get; set; }
     
        
    }
}
