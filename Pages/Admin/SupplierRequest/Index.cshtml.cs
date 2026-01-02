using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.Pages_Admin_SupplierRequest
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charpContext context)
        {
            _context = context;
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

            SupplierRequest = await query
                .OrderByDescending(sr => sr.CreatedAt)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToListAsync();
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

namespace E_commerce_c_charp.Pages_Admin_SupplierRequest
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public IList<SupplierRequest> SupplierRequest { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SupplierRequest = await _context.SupplierRequest.ToListAsync();
        }
    }
}
 */