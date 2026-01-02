using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_c_charp.Pages_Order;

public class IndexModel : PageModel
{
    private readonly E_commerce_c_charpContext _context;
    private readonly UserManager<User> _userManager;

    public IndexModel(E_commerce_c_charpContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public OrderDashboardViewModel Dashboard { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
            if (user == null /* || user.Id != UserId */) 
                return Unauthorized();

        if (user is null)
        {
            Dashboard = new OrderDashboardViewModel();
            return Page();
        }

        var orders = await _context.Order
            .Where(o => o.UserId == user.Id)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        Dashboard.Orders = orders.Select(o => new OrderSummaryViewModel
        {
            OrderNumber = o.OrderNumber,
            CreatedAt = o.CreatedAt,
            ItemsCount = o.Items.Sum(i => i.Quantity),
            StatusLabel = GetStatusDisplayName(o.Status),
            StatusCssClass = GetStatusCss(o.Status),
            DeliveryAddress = $"{o.Address}, {o.City}",
            Total = o.TotalAmount,
            EstimatedDelivery = o.EstimatedDelivery,
            Items = o.Items.Select(i => new OrderItemViewModel
            {
                ProductName = i.Product.Name,
                // Tu pourras construire la variante plus tard (taille, couleur…)
                Variant = null,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                ImageUrl = i.Product.ImageUrl
            }).ToList()
        }).ToList();

        Dashboard.OrdersCount = Dashboard.Orders.Count;
        Dashboard.TotalSpent = Dashboard.Orders.Sum(o => o.Total);
        Dashboard.Delivered = orders.Count(o => o.Status == Status.Delivered);
        Dashboard.InProgress = orders.Count(o => o.Status is Status.Pending or Status.Completed);
        Dashboard.ItemsPurchased = Dashboard.Orders.Sum(o => o.ItemsCount);
        return Page();
    }

    private static string GetStatusCss(Status status) =>
        status switch
        {
            Status.Pending   => "bg-warning text-dark",
            Status.Cancelled => "bg-danger",
            Status.Completed => "bg-primary",
            Status.Delivered => "bg-success",
            _                => "bg-secondary"
        };

    private static string GetStatusDisplayName(Status status)
    {
        var field = status.GetType().GetField(status.ToString());
        var attr = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                        .Cast<DisplayAttribute>()
                        .FirstOrDefault();
        return attr?.Name ?? status.ToString();
    }
}
