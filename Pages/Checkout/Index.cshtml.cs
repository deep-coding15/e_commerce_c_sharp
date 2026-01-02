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
                            .FirstOrDefaultAsync(c => c.UserId == user.Id && c.IsActive == true);

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
                            .FirstOrDefaultAsync(c => c.UserId == user.Id && c.IsActive);

            if (cart is null || cart.Items == null || !cart.Items.Any())
            {
                // panier vide : à toi de décider quoi faire (rediriger, message, etc.)
                //Order.Items = new List<CartItemViewModel>();
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
            order.Status = Status.Completed;
            //order.OrderNumber = $"ORD-{DateTime.Now:yyMMdd}-{order.Id:D4}";
           
            // Format : ORD-63871234567890-0001
            //order.OrderNumber = $"ORD-{DateTime.UtcNow.Ticks}-{order.Id:D4}";
            // Formater après coup (ex: ORD-2026-10254)

            // 2. Mapper les lignes : CheckoutViewModel.Items -> Order.Items
            order.Items = Order.Items
                            .Select(i => _mapper.Map<E_commerce_c_charp.Models.OrderItem>(i)).ToList();
            order.OrderNumber = "";
            // 3. Sauvegarder en BDD
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            order.OrderNumber = $"ORD-{DateTime.Now:yyMMdd}-{order.Id:D6}";
            //_context.Order.Update(order);
            await _context.SaveChangesAsync();
            
            // 4. Gestion  automatique des stocks
            foreach (var OrderItem in order.Items)
            {
                await _context.Product.Where(p => p.Id == OrderItem.ProductId)
                .ExecuteUpdateAsync(setters => 
                    setters.SetProperty(p => p.StockQuantity, p=> p.StockQuantity - OrderItem.Quantity)
                );
            }

            // 5. Vider le panier 
            // make the cart inactive directly in the database with a filter
            await _context.Cart
                .Where(c => c.UserId == user.Id && c.IsActive)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(c => c.IsActive, false)
                );

            //await _context.Cart.Where(c => c.UserId == user.Id).ExecuteDeleteAsync();


            // 4. Rediriger vers la page de reçu avec l'Id de la commande
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
