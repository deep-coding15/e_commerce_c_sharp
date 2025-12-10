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

namespace E_commerce_c_charp.Pages_Order
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charp.Data.E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charp.Data.E_commerce_c_charpContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? OrderStatus { get; set; } = default!;
        [BindProperty(SupportsGet = false)]
        public IList<Order> Order { get; set; } = default!;

        public IEnumerable<SelectListItem>? SelectListsStatus { get; set; }
        public async Task OnGetAsync()
        {
            IQueryable<Status> queryStatus = from o in _context.Order orderby o.Status select o.Status;

            var orders = from o in _context.Order select o;

                    //Console.WriteLine("if reussit une");
            
                    //Console.WriteLine("if reussit une");
            if (Enum.TryParse<Status>(OrderStatus, true, out var parsedStatus))
            {
                Console.WriteLine("if reussit");
                orders = orders.Where(or => or.Status == parsedStatus);
                Console.WriteLine("if order reussit");
            }

            SelectListsStatus = new SelectList(Enum.GetValues(typeof(Status))
                .Cast<Status>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                }), 
                "Value", 
                "Text");
            //SelectListsStatus = new SelectList(await queryStatus.Distinct().ToListAsync());
            Order = await orders.ToListAsync();
        }
    }
}
