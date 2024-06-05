using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkoButik.Models
{
    public class Campaign
    {
        [Key]
        public int CampaignId { get; set; }
        [StringLength(20, MinimumLength = 5)]
        public string CampaignName { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal CampaignAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
