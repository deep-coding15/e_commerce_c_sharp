using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Pages_Cart
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        private readonly UserManager<User> _userManager;
        public IList<Product> Product { get; set; } = default!;
        public IList<CartItemViewModel> CartItemViewModel { get; set; } = default!;
        public IList<CartItem> CartItem { get; set; } = default!;
        public int NbArticles { get; set; } = 0; 
        public int NbProducts { get; set; } = 0;
        public decimal PrixHT { get; set; } = 0;
        public decimal PrixTva { get; set; } = 0;
        public IList<Cart> Cart { get; set; } = default!;
        public decimal PrixTTC { get; set; } = 0;

        public IndexModel(
            E_commerce_c_charp.Data.E_commerce_c_charpContext context,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory clientFactory,
            UserManager<User> userManager
        )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = clientFactory.CreateClient();
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null /* || user.Id != UserId */) 
                return Unauthorized();

            Cart = await _context.Cart
                .Include(c => c.User)
                .Where(u => u.UserId == user.Id)
                .ToListAsync();

            Product = await _context.CartItem
                .Where(ci => ci.Cart.UserId == user.Id)
                //.Include(ci => ci.Quantity)
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .Select(ci => ci.Product)
                .ToListAsync();
            
            CartItem = await _context.CartItem
                .Where(ci => ci.Cart.UserId == user.Id)
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync();
            
            CartItemViewModel = CartItem.Select(ci => new CartItemViewModel
            {
                Product = ci.Product,
                Quantity = ci.Quantity
            }).ToList();

            NbArticles = CartItem.Distinct().Count();
            PrixHT = CartItem.Sum(ci => ci.Quantity * ci.Product.Price);
            PrixTva = Order.TVA * PrixHT;
            PrixTTC = PrixHT + PrixTva;
            
            return Page();
        }
    }
}
