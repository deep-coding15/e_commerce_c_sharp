using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_Product
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        /* For GET requests it returns a list of movies to the Razor Page.*/
        public async Task OnGetAsync() 
        {
            Product = await _context.Product
                .Include(p => p.Category).ToListAsync();
        }
    }
}
