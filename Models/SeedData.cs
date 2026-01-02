using System.Threading.Tasks;
using E_commerce_c_charp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_c_charp.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Rôles
        string[] roles = { "Admin", "Client" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Admin
        var adminEmail = "admin@nkaa.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                Nom = "Administrateur",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        var user1Email = "client1@nkaa.local";
        var user1 = await userManager.FindByEmailAsync(user1Email);

        var user2Email = "client2@nkaa.local";
        var user2 = await userManager.FindByEmailAsync(user2Email);

        // User
        if (user1 == null)
        {
            user1 = new User
            {
                UserName = user1Email,
                Email = user1Email,
                Nom = "Client One",
                EmailConfirmed = true,
            };
            await userManager.CreateAsync(user1, "Client123");
            await userManager.AddToRoleAsync(user1, "Client");
        }
        if (user2 == null)
        {
            user2 = new User
            {
                UserName = user2Email,
                Email = user2Email,
                Nom = "Client Two",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user2, "Client123");
            await userManager.AddToRoleAsync(user2, "Client");
        }

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
                    new Category { Name = "Artisanat & sculptures" },
                    new Category { Name = "Textiles traditionnels" },
                    new Category { Name = "Bijoux culturels" },
                    new Category { Name = "Instruments de musique" },
                    new Category { Name = "Décoration intérieure" },
                    new Category { Name = "Cosmétiques naturels" },
                    new Category { Name = "Alimentation locale" },
                    new Category { Name = "Objets rituels & symboliques" }
                );
                context.SaveChanges();
            }

            // =========================
            // SUPPLIERS
            // =========================
            if (!context.Supplier.Any())
            {
                context.Supplier.AddRange(
                    new Supplier
                    {
                        Name = "Coopérative Amazigh Atlas",
                        Country = "Maroc",
                        CulturalOrigin = "Amazigh",
                        ContactPhone = "+212600000001",
                        ContactEmail = "contact@amazigh-atlas.ma",
                        Description = "Artisanat amazigh : tapis, bijoux, poterie et cosmétiques naturels.",
                        Status = "Approved",
                        CreatedAt = DateTime.Now
                    },
                    new Supplier
                    {
                        Name = "Atelier Peul Sahel",
                        Country = "Sénégal",
                        CulturalOrigin = "Peul",
                        ContactPhone = "+221770000002",
                        ContactEmail = "contact@peulsahel.sn",
                        Description = "Textiles, bijoux et objets culturels peuls.",
                        Status = "Approved",
                        CreatedAt = DateTime.Now
                    },
                    new Supplier
                    {
                        Name = "Maison Baoulé",
                        Country = "Côte d’Ivoire",
                        CulturalOrigin = "Baoulé",
                        ContactPhone = "+22501020304",
                        ContactEmail = "baoule@culture.ci",
                        Description = "Masques, statues et objets rituels baoulé.",
                        Status = "Approved",
                        CreatedAt = DateTime.Now
                    },
                    new Supplier
                    {
                        Name = "Yoruba Heritage",
                        Country = "Nigeria",
                        CulturalOrigin = "Yoruba",
                        ContactPhone = "+234801111111",
                        ContactEmail = "contact@yorubaheritage.ng",
                        Description = "Objets traditionnels et textiles yoruba.",
                        Status = "Approved",
                        CreatedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
            }


            // ======================
            // Seed Products
            // ======================
            if (!context.Product.Any())
            {
                var artisanat = context.Category.First(c => c.Name == "Artisanat & sculptures").Id;
                var textile = context.Category.First(c => c.Name == "Textiles traditionnels").Id;
                var bijoux = context.Category.First(c => c.Name == "Bijoux culturels").Id;
                var musique = context.Category.First(c => c.Name == "Instruments de musique").Id;
                var cosmetique = context.Category.First(c => c.Name == "Cosmétiques naturels").Id;

                var amazigh = context.Supplier.First(s => s.Name.Contains("Amazigh")).Id;
                var peul = context.Supplier.First(s => s.Name.Contains("Peul")).Id;
                var baoule = context.Supplier.First(s => s.Name.Contains("Baoulé")).Id;
                var yoruba = context.Supplier.First(s => s.Name.Contains("Yoruba")).Id;

                context.Product.AddRange(
                     new Product
                     {
                         Name = "Tapis Amazigh fait main",
                         Description = "Tapis amazigh traditionnel tissé à la main dans le Haut Atlas.",
                         ImageUrl = "https://example.com/tapis_amazigh.jpg",
                         Price = 1200,
                         StockQuantity = 10,
                         CategoryId = artisanat,
                         Rating = 4.8m,
                         Brand = "Amazigh",
                         Sku = "NK-AMZ-001",
                         IsFeatured = true,
                         SupplierId = amazigh
                     },
                    new Product
                    {
                        Name = "Boubou Peul brodé",
                        Description = "Boubou traditionnel peul cousu et brodé à la main.",
                        ImageUrl = "https://example.com/boubou_peul.jpg",
                        Price = 650,
                        StockQuantity = 15,
                        CategoryId = textile,
                        Rating = 4.6m,
                        Brand = "Peul",
                        Sku = "NK-PEU-002",
                        IsFeatured = false,
                        SupplierId = peul
                    },
                    new Product
                    {
                        Name = "Masque Baoulé rituel",
                        Description = "Masque cérémoniel utilisé lors des rituels traditionnels baoulé.",
                        ImageUrl = "https://example.com/masque_baoule.jpg",
                        Price = 950,
                        StockQuantity = 5,
                        CategoryId = artisanat,
                        Rating = 4.9m,
                        Brand = "Baoulé",
                        Sku = "NK-BAO-003",
                        IsFeatured = true,
                        SupplierId = baoule
                    },
                    new Product
                    {
                        Name = "Tenue Yoruba Aso-Oke",
                        Description = "Tenue yoruba en tissu Aso-Oke portée lors des cérémonies.",
                        ImageUrl = "https://example.com/aso_oke.jpg",
                        Price = 780,
                        StockQuantity = 12,
                        CategoryId = textile,
                        Rating = 4.7m,
                        Brand = "Yoruba",
                        Sku = "NK-YOR-004",
                        IsFeatured = true,
                        SupplierId = yoruba
                    },
                    new Product
                    {
                        Name = "Djembe africain",
                        Description = "Djembe sculpté à la main avec peau naturelle.",
                        ImageUrl = "https://example.com/djembe.jpg",
                        Price = 990,
                        StockQuantity = 8,
                        CategoryId = musique,
                        Rating = 4.8m,
                        Brand = "Afrique de l’Ouest",
                        Sku = "NK-DJM-005",
                        IsFeatured = true,
                        SupplierId = baoule
                    },
                    new Product
                    {
                        Name = "Huile d’argan pure",
                        Description = "Huile d’argan marocaine pressée à froid.",
                        ImageUrl = "https://example.com/argan.jpg",
                        Price = 190,
                        StockQuantity = 40,
                        CategoryId = cosmetique,
                        Rating = 4.9m,
                        Brand = "Amazigh",
                        Sku = "NK-ARG-006",
                        IsFeatured = true,
                        SupplierId = amazigh
                    },
                    // ===== BIJOUX =====
                    new Product
                    {
                        Name = "Bracelet Massaï perlé",
                        Description = "Bracelet traditionnel massaï en perles colorées.",
                        ImageUrl = "https://example.com/bracelet_massai.jpg",
                        Price = 220,
                        StockQuantity = 30,
                        CategoryId = bijoux,
                        Rating = 4.6m,
                        Brand = "Massaï",
                        Sku = "NK-MAS-007",
                        IsFeatured = false,
                        SupplierId = peul
                    },
                    new Product
                    {
                        Name = "Boucles d’oreilles Ashanti",
                        Description = "Boucles en laiton inspirées de l’art Ashanti.",
                        ImageUrl = "https://example.com/boucles_ashanti.jpg",
                        Price = 180,
                        StockQuantity = 25,
                        CategoryId = bijoux,
                        Rating = 4.7m,
                        Brand = "Ashanti",
                        Sku = "NK-ASH-008",
                        IsFeatured = false,
                        SupplierId = baoule
                    },
                    // ===== DECORATION =====
                    new Product
                    {
                        Name = "Statue Dogon sculptée",
                        Description = "Statue dogon en bois utilisée dans la tradition spirituelle.",
                        ImageUrl = "https://example.com/statue_dogon.jpg",
                        Price = 1100,
                        StockQuantity = 6,
                        CategoryId = artisanat,
                        Rating = 4.9m,
                        Brand = "Dogon",
                        Sku = "NK-DOG-009",
                        IsFeatured = true,
                        SupplierId = baoule
                    },
                    // ===== TEXTILE =====
                    new Product
                    {
                        Name = "Pagne Wax africain",
                        Description = "Tissu wax coloré pour vêtements traditionnels.",
                        ImageUrl = "https://example.com/wax.jpg",
                        Price = 95,
                        StockQuantity = 100,
                        CategoryId = textile,
                        Rating = 4.5m,
                        Brand = "Wax Afrique",
                        Sku = "NK-WAX-010",
                        IsFeatured = false,
                        SupplierId = yoruba
                    },
                    // ===== OBJETS RITUELS =====
                    new Product
                    {
                        Name = "Amulette protectrice Touareg",
                        Description = "Amulette traditionnelle portée pour la protection spirituelle.",
                        ImageUrl = "https://example.com/amulette_touareg.jpg",
                        Price = 320,
                        StockQuantity = 20,
                        CategoryId = artisanat,
                        Rating = 4.8m,
                        Brand = "Touareg",
                        Sku = "NK-TOU-011",
                        IsFeatured = true,
                        SupplierId = amazigh
                    }
                );

                context.SaveChanges();
            }

            /* =========================
            UTILISATEURS
            ========================= */


            /* =========================
                    PANIER
                    ========================= */
            //var user1 = await userManager.FindByEmailAsync("client1@test.com");
            if (!context.Cart.Any())
            {
                context.Cart.Add(
                    new Cart
                    {
                        UserId = user1.Id,
                    }
                );
                context.Cart.Add(
                    new Cart
                    {
                        UserId = user2.Id,
                    }
                );
                context.SaveChanges();
            }

            var cart1 = context.Cart.First(c => c.UserId == user1.Id);
            var cart2 = context.Cart.First(c => c.UserId == user2.Id);
            
            var product1 = context.Product.First(p => p.Sku == "NK-AMZ-001");
            var product2 = context.Product.First(p => p.Sku == "NK-PEU-002");
            var product3 = context.Product.First(p => p.Sku == "NK-MAS-007");
            
            if (!context.CartItem.Any())
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
                    },
                    new CartItem ()
                    {
                        CartId = cart2.Id,
                        ProductId = product3.Id,
                        Quantity = 1
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
                        OrderNumber = "#00001",
                        UserId = user1.Id,
                        CreatedAt = DateTime.Now.AddDays(-3),
                        Status = Status.Completed,
                        TotalAmount = product1.Price + (product2.Price * 2)
                    },
                    new Order
                    {
                        OrderNumber = "#00002",
                        UserId = user2.Id,
                        CreatedAt = DateTime.Now.AddDays(-1),
                        Status = Status.Pending,
                        TotalAmount = product3.Price
                    }
                );

                context.SaveChanges();
            }

            // ======================
            // Seed Order Items
            // ======================
            if (!context.OrderItem.Any())
            {
                var order1 = context.Order.FirstOrDefault(o => o.UserId == user1.Id);
                var order2 = context.Order.FirstOrDefault(o => o.UserId == user2.Id);
                
                context.OrderItem.AddRange(
                    new OrderItem
                    {
                        OrderId = order1.Id,
                        ProductId = product1.Id,
                        Quantity = 1,
                        UnitPrice = product1.Price
                    },
                    new OrderItem
                    {
                        OrderId = order1.Id,
                        ProductId = product3.Id,
                        Quantity = 2,
                        UnitPrice = product3.Price
                    },
                    new OrderItem
                    {
                        OrderId = order2.Id,
                        ProductId = product2.Id,
                        Quantity = 1,
                        UnitPrice = product2.Price
                    }
                );
                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}