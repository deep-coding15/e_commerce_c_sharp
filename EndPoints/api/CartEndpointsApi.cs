using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models.Requests;

namespace E_commerce_c_charp.EndPoints;
public static class CartEndpointsApi
{
    public static void MapCartEndpointsApi(this WebApplication app)
    {
        var cartItems = app.MapGroup("/api/Cart");

        cartItems.MapPost("add", async (
            [FromBody] AddToCartRequest req,
            E_commerce_c_charpContext db,
            UserManager<User> userManager,
            HttpContext http) => {
            if (req is null || req.ProductId <= 0 || req.Quantity <= 0)
                return Results.BadRequest(new { success = false, message = "Produit invalide !" });

            var user = await userManager.GetUserAsync(http.User);
            if (user is null) return Results.Unauthorized();

            var product = await db.Product.FindAsync(req.ProductId);
            if (product is null)
                return Results.NotFound(new { success = false, message = "Produit introuvable." });

            var cart = await db.Cart.Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart is null)
            {
                cart = new Cart { UserId = user.Id, Items = new List<CartItem>() };
                db.Cart.Add(cart);
            }

            var line = cart.Items.FirstOrDefault(i => i.ProductId == req.ProductId);
            if (line is null)
                cart.Items.Add(new CartItem { ProductId = req.ProductId, Quantity = req.Quantity });
            else
                line.Quantity += req.Quantity;

            await db.SaveChangesAsync();

            return Results.Ok(new { success = true });
        });

        cartItems.MapGet("", async (
            UserManager<User> userManager,
            HttpContext http
        ) =>
        {
            var user = await userManager.GetUserAsync(http.User);
            if (user is null) return Results.Unauthorized();

            return Results.Redirect($"/Cart/Index?UserId={user.Id}");
        });

        cartItems.MapGet("Details/{id:int}", (int id) => Results.Redirect($"/Cart/Details?id={id}"));
    }
}
