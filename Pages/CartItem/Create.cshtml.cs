using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_CartItem
{
    public class CreateModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;

        public CreateModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id");
        ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public CartItem CartItem { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CartItem.Add(CartItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
