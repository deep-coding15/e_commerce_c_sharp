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

namespace E_commerce_c_charp.Pages_Cart
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        public IList<Product> Product { get; set; } = default!;
        public IList<CartItemViewModel> CartItemViewModel { get; set; } = default!;
        public IList<CartItem> CartItem { get; set; } = default!;
        public int NbArticles { get; set; } = 0; 
        public int NbProducts { get; set; } = 0;
        public decimal PrixTotal { get; set; } = 0;

        public IndexModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public IList<Cart> Cart { get; set; } = default!;

        public async Task OnGetAsync(string UserId)
        {
            Cart = await _context.Cart
                .Include(c => c.User)
                .Where(u => u.UserId == UserId)
                .ToListAsync();

            Product = await _context.CartItem
                .Where(ci => ci.Cart.UserId == UserId)
                //.Include(ci => ci.Quantity)
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .Select(ci => ci.Product)
                .ToListAsync();
            
            CartItem = await _context.CartItem
                .Where(ci => ci.Cart.UserId == UserId)
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Category)
                .ToListAsync();
            
            CartItemViewModel = CartItem.Select(ci => new CartItemViewModel
            {
                Product = ci.Product,
                Quantity = ci.Quantity
            }).ToList();

            NbArticles = CartItem.Distinct().Count();
            //NbProducts = cartItems.Distinct(ci => ci.ProductId).Sum(ci => ci.Quantity);
            PrixTotal = CartItem.Sum(ci => ci.Quantity * ci.Product.Price);

        }
    }
}
