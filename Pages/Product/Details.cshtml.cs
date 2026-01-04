using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Pages_Product
{
    public class DetailsModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        private readonly UserManager<User> _userManager;

        public DetailsModel(
            E_commerce_c_charp.Data.E_commerce_c_charpContext context,
            UserManager<User> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        public Product Product { get; set; } = default!;
        public Cart Cart { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            Cart cart = new Cart{};

            /* if (user is not null)
            {
                cart = await _context.Cart
                   .Include(c => c.User)
                   .FirstOrDefaultAsync(c => c.UserId == user.Id && c.IsActive);
                if (cart is null)
                    cart = new Cart
                    {
                        UserId = user.Id,
                        IsActive = true,
                    };
            } */
            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (product is not null)
            {
                Product = product;
                //Cart = 

                return Page();
            }

            return NotFound();
        }
    }
}
