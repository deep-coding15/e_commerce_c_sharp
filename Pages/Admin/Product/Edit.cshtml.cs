using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_Admin_Product
{
    public class EditModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly ILogger<Product> _logger;

        public EditModel(
            E_commerce_c_charpContext context,
            ILogger<Product> logger
        )
        {
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (Product == null || Product.Id == 0)
            {
                TempData["Error"] = "Erreur : Produit invalide.";
                return RedirectToPage("./Index");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(
                    await _context.Category.ToListAsync(),
                    "Id",
                    "Name",
                    Product.CategoryId
                );
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return Page();
            }

            try
            {
                var productFromDb = await _context.Product
                    .FirstOrDefaultAsync(p => p.Id == Product.Id);

                if (productFromDb == null)
                {
                    _logger.LogWarning($"Produit ID={Product.Id} introuvable.");
                    return NotFound();
                }

                // Mise à jour contrôlée
                _context.Entry(productFromDb).CurrentValues.SetValues(Product);

                // Propriétés spécifiques
                productFromDb.CategoryId = Product.CategoryId;
                productFromDb.IsFeatured = Product.IsFeatured;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Produit '{Product.Name}' modifié avec succès.");
                TempData["Success"] = $"Produit '{Product.Name}' modifié avec succès.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToPage("./Index");
        }   

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
