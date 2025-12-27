/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.Pages_Admin_Order;
using E_commerce_c_charp.ViewModels.Admin;

namespace E_commerce_c_charp.Pages_Admin_Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public DashboardViewModel Dashboard { get; set; } = new();

        public async Task OnGetAsync()
        {
            // TODO : remplir Dashboard.* à partir de ta base (EF Core)
        }
    }
}
 */