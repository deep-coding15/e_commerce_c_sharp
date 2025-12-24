using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.ViewModels;
using E_commerce_c_charp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Net;

namespace E_commerce_c_charp.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public IndexModel(E_commerce_c_charpContext context, UserManager<User> userManager, IMapper mapper)
        {
            _mapper = mapper;
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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                // En cas d’erreur de validation, on recalcule les totaux
                ComputeTotals();
                return Page();
            }

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

            // recalcul des totaux AVANT le mapping
            ComputeTotals();
            Console.WriteLine($"VM PrixHT={Order.PrixHT}, TVA={Order.PrixTVA}, TTC={Order.PrixTTC}");

            // 1. Mapper CheckoutViewModel -> Order
            var order = _mapper.Map<E_commerce_c_charp.Models.Order>(Order);
            order.UserId = user.Id;
            order.CreatedAt = DateTime.UtcNow;
            order.Status = Status.Pending;

            // 2. Mapper les lignes : CheckoutViewModel.Items -> Order.Items
            order.Items = Order.Items
                            .Select(i => _mapper.Map<E_commerce_c_charp.Models.OrderItem>(i)).ToList();

            // 3. Sauvegarder en BDD
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // 2. Vider le panier 
            // Deletes everything in the Cart table directly in the database with a filter
            await _context.Cart.Where(c => c.UserId == user.Id).ExecuteDeleteAsync();

            // 3. Rediriger vers la page de reçu avec l'Id de la commande
            return RedirectToPage("/Order/Receipt", new { id = order.Id });
            //return RedirectToPage("/Orders/Success");
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
