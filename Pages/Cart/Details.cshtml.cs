using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_Cart
{
    public class DetailsModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;

        public DetailsModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public Cart Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FirstOrDefaultAsync(m => m.Id == id);

            if (cart is not null)
            {
                Cart = cart;

                return Page();
            }

            return NotFound();
        }
    }
}
