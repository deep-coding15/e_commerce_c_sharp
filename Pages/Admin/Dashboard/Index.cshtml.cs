using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.ViewModels.Admin;
using NuGet.Protocol;

namespace E_commerce_c_charp.Pages_Admin_Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly ILogger<DashboardViewModel> _logger;

        public IndexModel(
            E_commerce_c_charpContext context,
            ILogger<DashboardViewModel> logger
        )
        {
            _context = context;
            _logger  = logger;
        }

        public DashboardViewModel Dashboard { get; set; } = new();

        public async Task OnGetAsync()
        {
            // ====== KPIs de base ======
            Dashboard.TotalOrders = await _context.Order.CountAsync();
            Dashboard.PendingOrders = await _context.Order
                .CountAsync(o => o.Status == Status.Pending);

            Dashboard.Revenue = await _context.Order
                .Where(o => o.Status == Status.Delivered)
                .SumAsync(o => (decimal?)o.PrixTTC) ?? 0m;

            Dashboard.AvgOrder = Dashboard.TotalOrders > 0
                ? Dashboard.Revenue / Dashboard.TotalOrders
                : 0;

            Dashboard.ProductsCount = await _context.Product.CountAsync();
            Dashboard.UsersCount = await _context.Users.CountAsync();

            // ====== Tendance des ventes par mois (6 derniers mois) ======
            var now = DateTime.UtcNow;
            var start = new DateTime(now.Year, now.Month, 1).AddMonths(-5); // 6 mois

            var salesByMonth = await _context.Order
                .Where(o => o.Status == Status.Delivered &&
                            o.CreatedAt >= start)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(x => x.PrixTTC)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            Dashboard.SalesMonths = salesByMonth
                .Select(x => new DateTime(x.Year, x.Month, 1).ToString("MMM"))
                .ToList();

            Dashboard.SalesTrend = salesByMonth
                .Select(x => (decimal)x.Total)
                .ToList();

            // ====== Statuts des commandes (camembert) ======
            var statusGroups = await _context.Order
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            Dashboard.OrderStatusCounts = statusGroups
                .ToDictionary(
                    x => x.Status.ToString(),   // ex: "Delivered"
                    x => x.Count
                );
            _logger.LogWarning($"order: {Dashboard.OrderStatusCounts.ToJson()}");
            // ====== Produits populaires (top 5) ======
            var topProducts = await _context.OrderItem
                .Include(oi => oi.Product)
                .GroupBy(oi => oi.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    SoldCount = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.SoldCount)
                .Take(5)
                .ToListAsync();

            int rank = 1;
            Dashboard.TopProducts = topProducts.Select(x => new TopProductViewModel
            {
                Rank = rank++,
                Name = x.Product.Name,
                SoldCount = x.SoldCount,
                Price = x.Product.Price
            }).ToList();

            // ====== Commandes récentes (5 dernières) ======
            var recentOrder = await _context.Order
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .ToListAsync();

            Dashboard.RecentOrders = recentOrder.Select(o => new RecentOrderViewModel
            {
                Number = o.OrderNumber,                      // ex: "#8595D0C"
                Date = o.CreatedAt,
                Total = o.PrixTTC,
                StatusLabel = o.Status.ToString()                 // ex: "Expédiée"
            }).ToList();
        }
    }
}
