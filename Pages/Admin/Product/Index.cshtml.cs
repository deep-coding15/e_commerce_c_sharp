using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_c_charp.ViewModels.Admin;

namespace E_commerce_c_charp.Pages_Admin_Product
{

    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly ILogger<Product> _logger;

        public IndexModel(
            E_commerce_c_charpContext context,
            ILogger<Product> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }


        // stats pour les cartes
        public int TotalProducts { get; set; }
        public int FeaturedCount { get; set; }
        public int TotalStock { get; set; }
        public decimal StockValue { get; set; }

        public List<Category> Categories { get; set; } = new();

        //public List<Category> Categories { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
        public async Task OnGetAsync()
        {
            var query = _context.Product
                .Include(p => p.Category)
                .OrderByDescending(p => p.IsFeatured)   // les vedette en premier
                                                        //.ThenByDescending(p => p.CreatedAt)   // puis éventuellement par date de création
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                query = query.Where(p =>
                    p.Name.Contains(Search) ||
                    p.Brand!.Contains(Search) //|| 
                                              //p.Description.Contains(Search)
                );
            }

            Products = await query.ToListAsync();

            TotalProducts = Products.Count;
            FeaturedCount = Products.Count(p => p.IsFeatured);
            TotalStock = Products.Sum(p => p.StockQuantity);
            StockValue = Products.Sum(p => p.Price * p.StockQuantity);

            Categories = await _context.Category.ToListAsync();

            Suppliers = await _context.Supplier.ToListAsync();
        }

        public Product Product { get; set; } = default!;
        //  Handler CORRIGÉ
        /* [BindProperty] */
        //public Product InputProduct { get; set; } = new();
        [BindProperty]
        public CreateProductViewModel InputProduct { get; set; } = new();

        /* public async Task<IActionResult> OnPostCreateProductAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"ModelState invalide. Erreurs: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                Categories = await _context.Category.ToListAsync(); // Recharge dropdowns
                return Page();
            }

            _logger.LogWarning($"Produit reçu: {InputProduct.Name}, Prix: {InputProduct.Price}");
            _logger.LogWarning($"ModelState valide: {ModelState.IsValid}");

            _context.Product.Add(InputProduct); // Utilise InputProduct
            await _context.SaveChangesAsync();

            TempData["Success"] = "Produit créé avec succès !";
            return RedirectToPage();
        }
 */
        /* public async Task<IActionResult> OnPostCreateProductAsync()
        {
            // ✅ Debug détaillé
            _logger.LogInformation($"POST: Sku={InputProduct.Sku}, Name={InputProduct.Name}");

            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                _logger.LogWarning($"ModelState KO: {errors}");
                Categories = await _context.Category.ToListAsync();
                Suppliers  = await _context.Supplier.ToListAsync();
                return Page();
            }

            _context.Product.Add(InputProduct);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Produit '{InputProduct.Name}' créé !";
            return RedirectToPage();
        }
     */
        public async Task<IActionResult> OnPostCreateProductAsync()
        {
            _logger.LogInformation("=== CREATE PRODUCT DÉBUT ===");
            _logger.LogWarning("Input: SKU={Sku}, Name={Name}, Price={Price}, CategoryId={CategoryId}, Rating={Rating}",
                InputProduct.Sku, InputProduct.Name, InputProduct.Price, InputProduct.CategoryId, InputProduct.Rating);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ VALIDATION ÉCHOUÉE - {ErrorCount} erreurs", ModelState.ErrorCount);
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        _logger.LogWarning("ModelState[{Key}]={Error}", error.Key, e.ErrorMessage);
                    }
                }

                LoadSelectLists();
                return Page();
            }
            try
            {
                _logger.LogDebug(" Mapping vers Product...");
                var product = new Product
                {
                    Sku = InputProduct.Sku,
                    Name = InputProduct.Name,
                    Description = InputProduct.Description,
                    ImageUrl = InputProduct.ImageUrl ?? string.Empty,
                    Price = InputProduct.Price,
                    StockQuantity = InputProduct.StockQuantity,
                    Brand = InputProduct.Brand,
                    CategoryId = InputProduct.CategoryId,
                    Rating = InputProduct.Rating,
                    SupplierId = InputProduct.SupplierId,
                    IsFeatured = InputProduct.IsFeatured
                };
                _logger.LogWarning("Product créé: ID={Id}, SKU={Sku}, Price={Price:F2}",
                    product.Id, product.Sku, product.Price);

                //  LOG 3: Save DB
                _logger.LogInformation("Ajout en DB...");
                _context.Product.Add(product);
                int saved = await _context.SaveChangesAsync();

                _logger.LogInformation("✅ SAUVEGARDÉ ! Rows affected: {Rows}, Product ID: {Id}", saved, product.Id);

                TempData["Success"] = $"Produit '{InputProduct.Name}' créé avec succès ! ID: {product.Id}";

                _logger.LogInformation("=== CREATE PRODUCT SUCCÈS - Redirect Index ===");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "❌ DB ERROR lors SaveChanges: {Message}", ex.Message);
                ModelState.AddModelError("", "Erreur base de données. SKU dupliqué ?");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ERREUR INATTENDUE: {Message}", ex.Message);
                ModelState.AddModelError("", "Erreur serveur. Réessayez.");
            }

            _logger.LogWarning("=== CREATE PRODUCT ÉCHOUÉ - Reload page ===");
            LoadSelectLists();
            return RedirectToPage("./Index");
        }

        private void LoadSelectLists()
        {
            InputProduct.Category = _context.Category
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .ToList();

            InputProduct.Supplier = _context.Supplier
                .Select(s => new SelectListItem
                {
                    Text = s.OrganizationName,
                    Value = s.Id.ToString(),
                })
                .ToList();
        }
    }
}


