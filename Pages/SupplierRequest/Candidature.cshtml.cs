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
    public class CandidatureModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public CandidatureModel(E_commerce_c_charpContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SupplierRequest SupplierRequest { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            SupplierRequest.IsReviewed = false;
            SupplierRequest.CreatedAt = DateTime.UtcNow;

            _context.Add(SupplierRequest);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Candidature soumise avec succès !";
            return RedirectToPage("./Success");
        }
    }
}