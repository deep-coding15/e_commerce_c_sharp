using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models.Requests;

namespace E_commerce_c_charp.EndPoints;

public static class OrderEndpointsApi
{
    public static void MapOrderEndpointsApi(this WebApplication app)
    {
        var OrderItems = app.MapGroup("/api/Order");

        OrderItems.MapGet("", async (
            UserManager<User> userManager,
            HttpContext http
        ) =>
        {
            var user = await userManager.GetUserAsync(http.User);
            if (user is null) return Results.Unauthorized();

            //return Results.Redirect($"/Order/Index?UserId={user.Id}");
            return Results.Redirect($"/Order/Index");
        });

        /* cartItems.MapGet("Details/{id:int}", (int id) => Results.Redirect($"/Cart/Details?id={id}"));

        OrderItem.MapGet("", async(
            [FromBody]
        ));

        OrderItems.MapPost("add", async (
            [FromBody] AddToCartRequest req,
            E_commerce_c_charpContext db,
            UserManager<User> userManager,
            HttpContext http) =>
        {
            if (req is null || req.ProductId <= 0 || req.Quantity <= 0)
                return Results.BadRequest(new { success = false, message = "Produit invalide !" });

            var user = await userManager.GetUserAsync(http.User);
            if (user is null) return Results.Unauthorized();

            var product = await db.Product.FindAsync(req.ProductId);
            if (product is null)
                return Results.NotFound(new { success = false, message = "Produit introuvable." });

            var cart = await db.Cart.Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart is null)
            {
                cart = new Cart { UserId = user.Id, Items = new List<CartItem>() };
                db.Cart.Add(cart);

                cart.Items.Add(new CartItem { ProductId = req.ProductId, Quantity = req.Quantity });

                var prixHT = product.Price;
                var prixTVA = prixHT * Order.TVA;
                var prixTTC = prixHT + prixTVA;

                return Results.Ok(new
                {
                    success = true,
                    PrixHT = prixHT,
                    PrixTVA = prixTVA,
                    PrixTTC = prixTTC,
                });
            }

            var line = cart.Items.FirstOrDefault(i => i.ProductId == req.ProductId);
            if (line is null)
                cart.Items.Add(new CartItem { ProductId = req.ProductId, Quantity = req.Quantity });
            else
                line.Quantity += req.Quantity;

            await db.SaveChangesAsync();

            var PrixHT = cart.Items
                .Where(ci => ci.Product != null)
                .Sum(ci => (ci?.Quantity ?? 0) * (ci?.Product?.Price ?? 0m));
            var PrixTVA = PrixHT * Order.TVA;
            var PrixTTC = PrixHT + PrixTVA;

            var result = new
            {
                success = true,
                PrixHT,
                PrixTVA,
                PrixTTC,
            };
            return Results.Ok(result);
        });
     */
    }
}