using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_User
{
    public class DeleteModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;

        public DeleteModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);

            if (user is not null)
            {
                User = user;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                User = user;
                _context.User.Remove(User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
