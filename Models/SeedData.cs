using E_commerce_c_charp.Data;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_c_charp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new E_commerce_c_charpContext(
            serviceProvider.GetRequiredService<DbContextOptions<E_commerce_c_charpContext>>()
        ))
        {
            if (context == null)
            {
                throw new ArgumentNullException("Null E_commerce_c_charp_context");
            }

            // ======================
            // Seed Categories
            // ======================
            if (!context.Category.Any())
            {
                context.Category.AddRange(
                    new Category { Name = "Smartphones" },
                    new Category { Name = "Laptops" },
                    new Category { Name = "Headphones" },
                    new Category { Name = "Gaming" }
                );
                context.SaveChanges();
            }

            // ======================
            // Seed Products
            // ======================
            if (!context.Product.Any())
            {
                var smartphones = context.Category.First(c => c.Name == "Smartphones");
                var laptops = context.Category.First(c => c.Name == "Laptops");
                var headphones = context.Category.First(c => c.Name == "Headphones");
                var gaming = context.Category.First(c => c.Name == "Gaming");

                context.Product.AddRange(
                    new Product
                    {
                        Name = "iPhone 15 Pro",
                        Description = "Apple smartphone with A17 Bionic chip",
                        Price = 1299.99m,
                        StockQuantity = 25,
                        CategoryId = smartphones.Id,
                        Rating = "R"
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S24",
                        Description = "Latest Samsung flagship smartphone",
                        Price = 1099.99m,
                        StockQuantity = 40,
                        CategoryId = smartphones.Id,
                        Rating = "R"
                    },
                    new Product
                    {
                        Name = "Dell XPS 15",
                        Description = "High-end laptop for work and productivity",
                        Price = 1899.99m,
                        StockQuantity = 15,
                        CategoryId = laptops.Id,
                        Rating = "R"
                    },
                    new Product
                    {
                        Name = "MacBook Air M3",
                        Description = "Ultra-portable Apple laptop",
                        Price = 1199.99m,
                        StockQuantity = 20,
                        CategoryId = laptops.Id,
                        Rating = "R"
                    },
                    new Product
                    {
                        Name = "Sony WH-1000XM5",
                        Description = "Noise-cancelling over-ear headphones",
                        Price = 399.99m,
                        StockQuantity = 60,
                        CategoryId = headphones.Id,
                        Rating = "R"
                    },
                    new Product
                    {
                        Name = "Logitech G Pro X",
                        Description = "Professional gaming headset",
                        Price = 149.99m,
                        StockQuantity = 30,
                        CategoryId = gaming.Id,
                        Rating = "R"
                    }
                );

                context.SaveChanges();
            }
            
            /* =========================
            UTILISATEURS
            ========================= */
            if(!context.User.Any())
            {                
                context.User.AddRange(
                    new User
                    {
                        Email = "client1@test.com",
                        PasswordHash = "HASHED_PASSWORD_1",
                        Nom = "Client One"
                    },
                    new User
                    {
                        Email = "client2@test.com",
                        PasswordHash = "HASHED_PASSWORD_2",
                        Nom = "Client Two"
                    }
                );
                context.SaveChanges();
            };

        /* =========================
                PANIER
                ========================= */
                var user1 = context.User.First(u => u.Email == "client1@test.com");
            if(!context.Cart.Any())
            {
                context.Cart.Add(
                    new Cart{
                        UserId = user1.Id,
                    }
                );
                context.SaveChanges();
            }
                var cart1 = context.Cart.First(c => c.UserId == user1.Id);
            var product1 = context.Product.First(p => p.Name == "iPhone 15 Pro");
            var product2 = context.Product.First(p => p.Name == "Sony WH-1000XM5");
            if(!context.CartItem.Any())
            {
                context.CartItem.AddRange(
                    new CartItem()
                    {
                        CartId = cart1.Id, 
                        ProductId = product1.Id,
                        Quantity = 1
                    },
                    new CartItem()
                    {
                        CartId = cart1.Id,
                        ProductId = product2.Id,
                        Quantity = 2
                    }
                );
                context.SaveChanges();
            }
        

            // ======================
            // Seed Orders
            // ======================
            if (!context.Order.Any())
            {
                context.Order.AddRange(
                    new Order
                    {
                        UserId = 1,
                        CreatedAt = DateTime.Now.AddDays(-3),
                        Status = Status.Completed,
                        TotalAmount = 1499.99m
                    },
                    new Order
                    {
                        UserId = 2,
                        CreatedAt = DateTime.Now.AddDays(-1),
                        Status = Status.Pending,
                        TotalAmount = 399.99m
                    }
                );

                context.SaveChanges();
            }

            // ======================
            // Seed Order Items
            // ======================
            if (!context.OrderItem.Any())
            {
                var iphone = context.Product.FirstOrDefault(p => p.Name == "iPhone 15 Pro");
                var sonyHeadphones = context.Product.FirstOrDefault(p => p.Name == "Sony WH-1000XM5");
                var order1 = context.Order.FirstOrDefault(o => o.UserId == 1);
                var order2 = context.Order.FirstOrDefault(o => o.UserId == 2);
                context.OrderItem.AddRange(
                    new OrderItem
                    {
                        OrderId   = order1.Id,
                        ProductId = iphone.Id,
                        Quantity  = 1,
                        UnitPrice = iphone.Price
                    },
                    new OrderItem
                    {
                        OrderId   = order2.Id,
                        ProductId = sonyHeadphones.Id,
                        Quantity  = 1,
                        UnitPrice = sonyHeadphones.Price
                    }
                );
                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}