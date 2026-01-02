using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_SupplierRequest
{
    public class DeleteModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;

        public DeleteModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SupplierRequest SupplierRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierrequest = await _context.SupplierRequest.FirstOrDefaultAsync(m => m.Id == id);

            if (supplierrequest is not null)
            {
                SupplierRequest = supplierrequest;

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

            var supplierrequest = await _context.SupplierRequest.FindAsync(id);
            if (supplierrequest != null)
            {
                SupplierRequest = supplierrequest;
                _context.SupplierRequest.Remove(SupplierRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
