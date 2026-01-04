using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.ViewModels;
using Microsoft.Extensions.Logging;

namespace E_commerce_c_charp.Pages_Landing
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<SupplierRequest> SupplierRequest { get; set; } = default!;

        [BindProperty]
        public CreateSupplierRequestViewModel Input { get; set; } = new();

        public IActionResult OnGet()
        {
            _logger.LogInformation("Page Devenir Fournisseur chargée - User: {UserId}, IP: {RemoteIp}", 
                User?.Identity?.Name ?? "Anonyme", HttpContext.Connection.RemoteIpAddress);
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            string userIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            string userId = User?.Identity?.Name ?? "Anonyme";

            _logger.LogInformation("Tentative soumission candidature - User: {UserId}, IP: {RemoteIp}, Email: {Email}", 
                userId, userIp, Input.Email);

            if (!ModelState.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var entry in ModelState)
                {
                    foreach (var e in entry.Value.Errors)
                    {
                        validationErrors.Add($"{entry.Key}: {e.ErrorMessage}");
                        _logger.LogWarning("VALIDATION [{Key}] = {Error}", entry.Key, e.ErrorMessage);
                    }
                }

                string errorSummary = string.Join(" | ", validationErrors);
                _logger.LogWarning("🚫 VALIDATION ÉCHOUÉE - User: {UserId}, IP: {RemoteIp}, Total: {Count}, Détails: {Errors}",
                    userId, userIp, ModelState.ErrorCount, errorSummary);

                TempData["ValidationErrors"] = $"Corrigez {ModelState.ErrorCount} erreur(s)";
                return Page();
            }

            try
            {
                var request = new SupplierRequest
                {
                    FullName = Input.FullName,
                    OrganizationName = Input.OrganizationName,
                    ActivityType = Input.ActivityType,
                    Country = Input.Country,
                    CulturalOrigin = Input.CulturalOrigin,
                    ProductTypes = Input.ProductTypes,
                    Description = Input.Description,
                    Phone = Input.Phone,
                    Email = Input.Email,
                    Website = Input.Website ?? "",
                    Facebook = Input.Facebook ?? "",
                    Instagram = Input.Instagram ?? "",
                    IsReviewed = false,
                    CreatedAt = DateTime.UtcNow  // ✅ Fix erreur NULL
                };

                request.CreatedAt = DateTime.UtcNow;

                _context.SupplierRequest.Add(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ CANDIDATURE OK - ID: {Id}, User: {UserId}, Email: {Email}", 
                    request.Id, userId, Input.Email);

                TempData["Success"] = "Candidature soumise ! Réponse sous 48h.";
                return RedirectToPage("/Product/Index");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "💥 DB ERROR - User: {UserId}, IP: {RemoteIp}, Email: {Email}, Input: {@Input}", 
                    userId, userIp, Input.Email, Input);
                ModelState.AddModelError("", "Erreur DB. Contactez support.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 UNEXPECTED ERROR - User: {UserId}, IP: {RemoteIp}, Input: {@Input}", 
                    userId, userIp, Input);
                ModelState.AddModelError("", "Erreur serveur. Équipe alertée.");
            }

            return RedirectToPage("./Product/Index");
        }
    }
}  
/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.ViewModels;
using Microsoft.Extensions.Logging;  // Ajout pour ILogger

namespace E_commerce_c_charp.Pages_Landing
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;
        private readonly ILogger<IndexModel> _logger;  // Injection logger

        public IndexModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<SupplierRequest> SupplierRequest { get; set; } = default!;

        [BindProperty]
        public CreateSupplierRequestViewModel Input { get; set; } = new();

        public IActionResult OnGet()
        {
            _logger.LogInformation("Page Devenir Fournisseur chargée - User: {UserId}, IP: {RemoteIp}",
                User?.Identity?.Name ?? "Anonyme", HttpContext.Connection.RemoteIpAddress);
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            string userIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            string userId = User?.Identity?.Name ?? "Anonyme";

            // Log tentative soumission
            _logger.LogWarning("Tentative soumission candidature fournisseur - User: {UserId}, IP: {RemoteIp}, Email: {Email}",
                userId, userIp, Input.Email);

            if (!ModelState.IsValid)
            {
                var validationErrors = new List<string>();
                foreach (var error in ModelState)
                {
                    foreach (var e in error.Value.Errors)
                    {
                        _logger.LogWarning("ModelState[{Key}]={Error}", error.Key, e.ErrorMessage);
                    }
                }

                string errorSummary = string.Join(" | ", validationErrors);

                _logger.LogWarning("🚫 VALIDATION ÉCHOUÉE - User: {UserId}, IP: {RemoteIp}, Champs: {Total}, Détails: {Errors}, Input: {@Input}",
                    userId, userIp, ModelState.ErrorCount, errorSummary, Input);

                /* _logger.LogWarning("🚫 VALIDATION ÉCHOUÉE - User: {UserId}, IP: {RemoteIp}, Champs: {Total}, Détails: {Errors}, Input: {@Input}",
                    userId, userIp, ModelState.ErrorCount, errorSummary, Input);
 * /
                TempData["ValidationErrors"] = $"Corrigez {ModelState.ErrorCount} erreur(s)";
                return Page();
            }

            try
            {
                var request = new SupplierRequest
                {
                    FullName = Input.FullName,
                    OrganizationName = Input.OrganizationName,
                    ActivityType = Input.ActivityType,
                    Country = Input.Country,
                    CulturalOrigin = Input.CulturalOrigin,
                    ProductTypes = Input.ProductTypes,
                    Description = Input.Description,
                    Phone = Input.Phone,
                    Email = Input.Email,
                    Website = Input.Website,
                    Facebook = Input.Facebook,
                    Instagram = Input.Instagram,
                    IsReviewed = false,
                    CreatedAt = DateTime.UtcNow  // Ajout timestamp création
                };

                _context.SupplierRequest.Add(request);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Candidature fournisseur créée avec succès - ID: {Id}, User: {UserId}, IP: {RemoteIp}, Email: {Email}",
                    request.Id, userId, userIp, Input.Email);

                TempData["Success"] = "Candidature soumise ! Nous vous contactons sous 48h.";
                return RedirectToPage("/Index");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Erreur DB lors création candidature - User: {UserId}, IP: {RemoteIp}, Email: {Email}",
                    userId, userIp, Input.Email);
                ModelState.AddModelError("", "Erreur sauvegarde. Réessayez ou contactez-nous.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur inattendue soumission candidature - User: {UserId}, IP: {RemoteIp}, Email: {Email}",
                    userId, userIp, Input.Email);
                ModelState.AddModelError("", "Erreur interne. Nos équipes sont informées.");
            }

            return Page();
        }
    }
}
 */