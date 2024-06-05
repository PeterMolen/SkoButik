using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkoButik.Models;

namespace SkoButik.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // Product Part
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        //Order Part
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        //Contact
        public DbSet<Contact> Contact { get; set; }


        //modelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure primary keys / 
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);

            // Configure relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Orders)
                .HasForeignKey(oi => oi.FkOrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Products)
                .WithMany()
                .HasForeignKey(oi => oi.FkProductId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ApplicationUser)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            base.OnModelCreating(modelBuilder);

            //////////////////ÍNVENTORY//////////////////////////

            // Inventory
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Products)
                .WithMany(p => p.Inventories)
                .HasForeignKey(i => i.FkProductId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Sizes)
                .WithMany(s => s.Inventories)
                .HasForeignKey(i => i.FkSizeId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction

            // ShoppingCartItem
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(sci => sci.FkProductId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Size)
                .WithMany(s => s.ShoppingCartItems)
                .HasForeignKey(sci => sci.FkSizeId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction

            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Products)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.FkProductId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Sizes)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(oi => oi.FkSizeId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict or NoAction

        }

    }
        
}
