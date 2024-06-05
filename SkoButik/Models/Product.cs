using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SkoButik.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Productname must be between 2 and 50 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [StringLength(60, MinimumLength = 10 )]
        public string? Description { get; set; }

        [StringLength (60, MinimumLength = 1 )]
        public string? ImageUrl { get; set; } //ändra till Byte[]???
       
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [NotMapped]
        public decimal AdjustedPrice
        {
            get => Price - (Campaign?.CampaignAmount * 0.01m * Price ?? 0m);
        }

        // One to many = Campaign 1---[ Products
        [ForeignKey("Campaign")]
        public int FkCampaignId { get; set; }
        public Campaign? Campaign { get; set; } = null;

        // One to many = Brand 1---[ Products
        [ForeignKey("Brand")]
        public int FkBrandId { get; set; }
        public Brand? Brand { get; set; }

        //public ICollection<Inventory>? Inventory { get; set; }

        public ICollection<Inventory>? Inventories { get; set; }
        public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
