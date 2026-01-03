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

namespace E_commerce_c_charp.Pages_Admin_SupplierRequest
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly ILogger<SupplierRequest> _logger;

        public IndexModel(
            E_commerce_c_charpContext context,
            ILogger<SupplierRequest> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<SupplierRequest> SupplierRequest { get; set; } = default!;

        // Filtres
        public string CurrentFilter { get; set; } = string.Empty;
        public string StatusFilter { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages => (int)Math.Ceiling((double)SupplierRequest.Count / 10);

        public async Task OnGetAsync(string? search, string? status, int page = 1)
        {
            CurrentPage = page;
            CurrentFilter = search ?? string.Empty;
            StatusFilter = status ?? string.Empty;

            var query = _context.SupplierRequest.AsQueryable();

            // Recherche
            if (!string.IsNullOrWhiteSpace(CurrentFilter))
            {
                query = query.Where(sr => sr.FullName.Contains(CurrentFilter) ||
                                        sr.OrganizationName.Contains(CurrentFilter) ||
                                        sr.Email.Contains(CurrentFilter));
            }

            // Statut
            if (StatusFilter == "pending")
            {
                query = query.Where(sr => !sr.IsReviewed);
            }
            else if (StatusFilter == "reviewed")
            {
                query = query.Where(sr => sr.IsReviewed);
            }
            else if (StatusFilter == "archived")
            {
                query = query.Where(sr => sr.IsArchived);
            }

            SupplierRequest = await query
                    .OrderByDescending(sr => sr.IsReviewed)      // Approuvés (true=1) en HAUT
                    .ThenBy(sr => sr.IsArchived)                 // Archivés (true=1) en BAS
                    .ThenByDescending(sr => sr.CreatedAt)        // Récents en 1er (même statut)
                    /* .Skip((pageNumber - 1) * pageSize)           // Pagination activée
                    .Take(pageSize) */
                    .ToListAsync();
        }

        public async Task<IActionResult> OnPostReviewAsync(int id)
        {
            var request = await _context.SupplierRequest.FindAsync(id);
            if (request == null) return NotFound();

            request.IsReviewed = true;
            request.ReviewedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Candidature {Id} marquée examinée par {User}", id, User.Identity?.Name);
            TempData["Success"] = "Candidature examinée !";
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostArchiveAsync(int id)
        {
            var request = await _context.SupplierRequest.FindAsync(id);
            if (request == null) return NotFound();

            request.IsArchived = true;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Candidature {Id} archivée par {User}", id, User.Identity?.Name);
            TempData["Success"] = "Candidature archivée !";
            return RedirectToPage("./Index");
        }
    }
}