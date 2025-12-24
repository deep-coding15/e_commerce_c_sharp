using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Authorization;

namespace E_commerce_c_charp.Pages_Admin_Product{

    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }


        // stats pour les cartes
        public int TotalProducts { get; set; }
        public int FeaturedCount { get; set; }
        public int TotalStock { get; set; }
        public decimal StockValue { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Product
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(p =>
                    p.Name.Contains(Search) ||
                    p.Brand!.Contains(Search));
            }

            Products = await query.ToListAsync();

            TotalProducts = Products.Count;
            FeaturedCount = Products.Count(p => p.IsFeatured);
            TotalStock = Products.Sum(p => p.StockQuantity);
            StockValue = Products.Sum(p => p.Price * p.StockQuantity);
        }
    }
}
