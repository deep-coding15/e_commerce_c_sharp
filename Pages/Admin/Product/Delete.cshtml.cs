using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Pages_Admin_Product
{
    public class DeleteModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        private readonly ILogger<Product> _logger;

        public DeleteModel(
            E_commerce_c_charp.Data.E_commerce_c_charpContext context,
            ILogger<Product> logger
        )
        {
            _context = context;
            _logger  = logger;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        public bool confirmDeleteChecked { get; set; } = false;
        public bool IsHardDelete { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (product is not null)
            {
                Product = product;

                return Page();
            }

            return NotFound();
        }

        /* public async Task<IActionResult> OnPostArchiveAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                Product = product;
                product.IsArchived = true;
                _context.Product.Update(product);  
                await _context.SaveChangesAsync();
                //_logger.LogInformation("✅ Product ID={Id} archivé (IsArchived=true)", id);
                TempData["Success"] = $"Produit '{product.Name}' archivé avec succès.";
    
            }

            return RedirectToPage("./Index");
        }
    
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                Product = product;
                product.IsArchived = true;
                _context.Product.Update(product);  
                await _context.SaveChangesAsync();
                //_logger.LogInformation("✅ Product ID={Id} archivé (IsArchived=true)", id);
                TempData["Success"] = $"Produit '{product.Name}' archivé avec succès.";
    
            }

            return RedirectToPage("./Index");
        }
     */
        /* public async Task<IActionResult> OnPostArchiveAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();

            product.IsArchived = true;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Produit archivé !";
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostHardDeleteAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Danger"] = "Produit supprimé définitivement !";
            return RedirectToPage("./Index");
        } */

        // ✅ HANDLER ARCHIVE
        [BindProperty]
        public int id {get; set;}
        public async Task<IActionResult> OnPostArchiveAsync(int id)
        {
            _logger.LogInformation("POST Archive ID={Id}", id);

            try
            {
                var product = await _context.Product.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Archive: Product ID={Id} non trouvé", id);
                    return NotFound();
                }

                // ✅ SOFT DELETE
                product.IsArchived = true;
                _context.Update(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Product ID={Id} archivé: {Name}", id, product.Name);
                TempData["Success"] = $"\"{product.Name}\" archivé avec succès.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur Archive ID={Id}", id);
                TempData["Error"] = "Erreur lors de l'archivage.";
            }

            return RedirectToPage("./Index");
        }

        // ✅ HANDLER HARD DELETE
        public async Task<IActionResult> OnPostHardDeleteAsync(int id)
        {
            _logger.LogInformation("POST HardDelete ID={Id}", id);

            try
            {
                var product = await _context.Product
                    .Include(p => p.OrderItems)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    _logger.LogWarning("HardDelete: Product ID={Id} non trouvé", id);
                    return NotFound();
                }

                // ✅ Vérif commandes
                if (product.OrderItems.Any())
                {
                    _logger.LogWarning("HardDelete bloqué: commandes liées ID={Id}", id);
                    TempData["Error"] = "Impossible: produit lié à des commandes.";
                    return Page();
                }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Product ID={Id} supprimé définitivement: {Name}", id, product.Name);
                TempData["Danger"] = $"\"{product.Name}\" supprimé définitivement.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur HardDelete ID={Id}", id);
                TempData["Error"] = "Erreur lors de la suppression.";
            }

            return RedirectToPage("./Index");
        }
    }
}
