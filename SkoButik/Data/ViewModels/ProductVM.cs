using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SkoButik.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int FkBrandId { get; set; }

        [Required]
        public int FkCampaignId { get; set; }

        public List<int> SizeIds { get; set; }
    }
}