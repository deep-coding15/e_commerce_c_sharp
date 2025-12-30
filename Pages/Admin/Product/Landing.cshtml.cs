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

public class LandingModel : PageModel
{
    private readonly E_commerce_c_charpContext _context;

    public LandingModel(E_commerce_c_charpContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> FeaturedProducts { get; set; } = Enumerable.Empty<Product>();
    public StatsModel Stats { get; set; } = new();

    public async Task OnGetAsync()
    {
        FeaturedProducts = await _context.Product
            .Include(p => p.Category)
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.Rating)
            .Take(6)
            .ToListAsync();

        Stats = new StatsModel
        {
            TotalProducts = await _context.Product.CountAsync(),
            TotalOrders = await _context.Order.CountAsync()
        };
    }

    public class StatsModel
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
    }
}
