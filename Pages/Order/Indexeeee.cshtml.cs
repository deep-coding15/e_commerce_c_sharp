using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Pages_Order
{
    public class IndexeModel : PageModel
    {
        public DateTime? EndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public List<Order> Orders { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        private readonly UserManager<User> _userManager;
        private readonly E_commerce_c_charpContext _context;

        public IndexeModel(
            UserManager<User> userManager,
            E_commerce_c_charpContext context
        )
        {
            _context = context;
        }



        public async Task OnGetAsync(string status, DateTime? startDate, DateTime? endDate)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return;

            var query = _context.Order
                .Where(o => o.UserId == user.Id)
                .Include(o => o.Items);

            if (!string.IsNullOrEmpty(status))
            {
                //query = query.Where(o => o.Status = status);
            }

            if (startDate.HasValue)
            {
                //query = query.Where(o => o.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                //query = query.Where(o => o.CreatedAt <= endDate.Value);
            }

            Orders = await query.ToListAsync();
        }
    }
}
