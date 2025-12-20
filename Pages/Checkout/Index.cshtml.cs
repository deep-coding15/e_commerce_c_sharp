using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.ViewModels;
using E_commerce_c_charp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(E_commerce_c_charpContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public CheckoutViewModel Order { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            // TODO : charger le panier réel depuis la BDD pour l'utilisateur connecté
            // Simulation d’items :
            var user = await _userManager.GetUserAsync(User); //ici, user = httpcontext.User
            if (user is null)
                return RedirectToPage("/Account/Login"); // à adapter

            var cart = await _context.Cart
                            .Include(i => i.Items)
                                .ThenInclude(u => u.Product)
                            .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart is null || cart.Items == null || !cart.Items.Any())
            {
                // panier vide : à toi de décider quoi faire (rediriger, message, etc.)
                Order.Items = new List<CartItemViewModel>();
                return Page(); // Panier vide
            }

            Order.Items = cart.Items
                .Select(ci => new CartItemViewModel
                {
                    Product = ci.Product,
                    Quantity = ci.Quantity
                    // TotalPrice est calculé automatiquement
                })
                .ToList();
            
            Order.FullName = user.UserName;
            Order.Email = user.Email;
            Order.Address = /* user.Address ?? */ "";
            Order.City = "";
            Order.Phone = user.PhoneNumber;

            ComputeTotals();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // En cas d’erreur de validation, on recalcule les totaux
                ComputeTotals();
                return Page();
            }

            // TODO : sauvegarder la commande (Order) en base, vider le panier, etc.

            return RedirectToPage("/Orders/Success");
        }

        private void ComputeTotals()
        {
            var prixHT = Order.Items.Sum(i => i.TotalPrice);
            Order.PrixHT = prixHT;
            Order.PrixTVA = prixHT * 0.2m;  // TVA 20 %
            Order.PrixTTC = Order.PrixHT + Order.PrixTVA;
        }
    }
}
