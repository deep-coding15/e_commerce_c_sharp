using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Pages_Cart
{
    public class CreateModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        private readonly UserManager<User> _userManager;
        public CreateModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_userManager.Users, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Cart Cart { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Cart.Add(Cart);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
