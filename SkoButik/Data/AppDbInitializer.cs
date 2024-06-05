using Microsoft.EntityFrameworkCore;
using SkoButik.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkoButik.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // Check if the Users table exists, if not, create the database
                if (!context.Users.Any())
                {
                    // Add initial users or do nothing if you want to register users manually
                }

                if (!context.Campaigns.Any())
                {
                    context.Campaigns.AddRange(new List<Campaign>()
                    {
                        new Campaign()
                        {
                            CampaignName = "No Campaign",
                            CampaignAmount = 0,
                            StartDate = new DateTime(2022,05,15),
                            EndDate = new DateTime(2026,06,01)

                        }
                    });
                    context.SaveChanges();
                }

                //Brands
                if (!context.Brands.Any())
                {
                    context.Brands.AddRange(new List<Brand>()
                    {
                        new Brand()
                        {
                            BrandName = "Nike"

                        },
                        new Brand()
                        {
                            BrandName = "Adidas"

                        },
                      new Brand()
                        {
                            BrandName = "Puma"

                        },
                      new Brand()
                        {
                            BrandName = "Vans"

                        }
                    });
                    context.SaveChanges();
                }
                //Produvers
                if (!context.Sizes.Any())
                {
                    context.Sizes.AddRange(new List<Size>()
                    {
                        new Size()
                        {
                            SizeName = "EU Men's Size 40"

                        },
                        new Size()
                        {
                            SizeName = "EU Men's Size 41"

                        },
                        new Size()
                        {
                            SizeName = "EU Men's Size 42"

                        },
                        new Size()
                        {
                            SizeName = "EU Men's Size 43"

                        },
                        new Size()
                        {
                            SizeName = "EU Wmn's Size 36"

                        },
                        new Size()
                        {
                            SizeName = "EU Wmn's Size 37"

                        },
                        new Size()
                        {
                            SizeName = "EU Wmn's Size 38"

                        },
                        new Size()
                        {
                            SizeName = "EU Wmn's Size 39"

                        }
                    });
                    context.SaveChanges();



                }
                //Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                        //Nike
                        new Product()
                        {
                            ProductName = "Nike Air Force 1 Low",
                            Description = "White Air Force 1, a classic shoe",
                            FkBrandId = 1,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Nike_airforce_low_BlackRed.jpg",
                            Price = 1500.00M
                        },
                         new Product()
                        {
                            ProductName = "Nike Air Force 07 high",
                            Description = "White and high",
                            FkBrandId = 1,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Nike_airforce_07High_white.jpg",
                            Price = 1699.00M
                        },
                        new Product()
                        {
                            ProductName = "Nike Jordan retro blue",
                            Description = "a blue classic style",
                            FkBrandId = 1,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Nike_airJordan_retro_blue.jpg",
                            Price = 2000.00M
                        },
                        new Product()
                        {
                            ProductName = "Nike Low White",
                            Description = "another classic low white",
                            FkBrandId = 1,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Nike_smeakers_low_white.jpg",
                            Price = 850.00M
                        },
                        new Product()
                        {
                            ProductName = "Nike white pink wmn",
                            Description = "White pink, woman model",
                            FkBrandId = 1,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Nike_whit_pink.jpg",
                            Price = 900.00M
                        },


                        //Adidas
                        new Product()
                        {
                            ProductName = "Adidas Superstars",
                            Description = "a superstar shoe for a superstar",
                            FkBrandId = 2,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Adidas_OrignialSuperstar.jpg",
                            Price = 999.00M

                        },
                        new Product()
                        {
                            ProductName = "Adidas Adizero wmn",
                            Description = "colorful shoe! Woman",
                            FkBrandId = 2,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Adidas2.jpg",
                            Price = 1999.00M

                        },
                         new Product()
                        {
                            ProductName = "Adidas campus",
                            Description = "Campus black",
                            FkBrandId = 2,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Adidas7.jpg",
                            Price = 899.00M

                        },
                         new Product()
                        {
                            ProductName = "Adidas green street",
                            Description = "Green and clean",
                            FkBrandId = 2,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Adidas1.jpg",
                            Price = 1200.00M

                        },



                        //Puma
                        new Product()
                        {
                            ProductName = "Puma Xray",
                            Description = "Xray colorful",
                            FkBrandId = 3,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Puma_Xray.jpg",
                            Price = 950.00M
                        },
                        new Product()
                        {
                            ProductName = "Puma red´wmn",
                            Description = "Red puma shoe for women",
                            FkBrandId = 3,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Puma_red_wmn.jpg",
                            Price = 1700.00M
                        },
                        new Product()
                        {
                            ProductName = "Puma brown wmn",
                            Description = "Brown street shoe for women",
                            FkBrandId = 3,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Puma_brown_wmn.jpg",
                            Price = 950.00M
                        },
                         new Product()
                        {
                            ProductName = "Puma midnight pink",
                            Description = "midnight pink like the nightfall",
                            FkBrandId = 3,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Puma_midnight_pink.jpg",
                            Price = 950.00M
                        },
                        new Product()
                        {
                            ProductName = "Puma OrangeBlack",
                            Description = "Orange and black puma shoe",
                            FkBrandId = 3,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Puma_OrangeBlack.jpg",
                            Price = 950.00M
                        },

                        //vans
                        new Product()
                        {
                            ProductName = "Vans OldSkool",
                            Description = "The most bought vans-shoe",
                            FkBrandId = 4,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Vans_OldSkool.jpg",
                            Price = 1200.00M
                        },
                        new Product()
                        {
                            ProductName = "Vans blu sk8 ",
                            Description = "skater shoe",
                            FkBrandId = 4,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Vans_blu_sk8.jpg",
                            Price = 1250.00M
                        },
                         new Product()
                        {
                            ProductName = "Vans Red sk8",
                            Description = "red skater shoe",
                            FkBrandId = 4,
                            FkCampaignId = 1,
                            ImageUrl = "/images/Vans4.jpg",
                            Price = 1400.00M
                        },
                      new Product()
                        {
                            ProductName = "Vans Sk8 High",
                            Description = "sk8 high, brown",
                            FkBrandId = 4,
                            FkCampaignId = 1,
                            ImageUrl = "/images/VansSk8_High.jpg",
                            Price = 2000.00M
                        }

                    });
                    context.SaveChanges();


                    // Inventory
                    if (!context.Inventories.Any())
                    {
                        var inventories = new List<Inventory>();

                        for (int productId = 1; productId <= 18; productId++)
                        {
                            for (int sizeId = 1; sizeId <= 8; sizeId++)
                            {
                                inventories.Add(new Inventory()
                                {
                                    FkProductId = productId,
                                    FkSizeId = sizeId,
                                    Quantity = 10
                                });
                            }
                        }

                        context.Inventories.AddRange(inventories);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}