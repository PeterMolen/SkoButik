namespace SkoButik.Models
{
    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public ApplicationUser OrderedBy { get; set; }
        public decimal TotalPrice { get; set; }
        public string Distance { get; set; }
    }
}