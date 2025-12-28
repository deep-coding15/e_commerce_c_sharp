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
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using NuGet.Protocol;

namespace E_commerce_c_charp.Pages_Admin_Order;

public class IndexModel : PageModel
{
    private readonly E_commerce_c_charpContext _context;
    private readonly ILogger<OrderDetailViewModel> _logger;

    public IndexModel(
        E_commerce_c_charpContext context,
        ILogger<OrderDetailViewModel> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public string AdminName { get; set; } = "Administrateur";

    [BindProperty(SupportsGet = true)]
    public Status? FilterStatus { get; set; }

    // Liste des statuts pour le filtre + le dropdown par ligne
    public List<SelectListItem> StatusOptions { get; set; } = default!;
    public int CountPending { get; set; }
    public int CountCancelled { get; set; }
    public int CountCompleted { get; set; }
    public int CountDelivered { get; set; }

    public int CountConfirmed { get; set; }
    public int CountProcessing { get; set; }
    public int CountShipped { get; set; }

    public List<OrderRowViewModel> Orders { get; set; } = new();

    //---------------------------------
    // GET : chargement de la page
    //---------------------------------
    public async Task OnGetAsync()
    {
        StatusOptions = Enum
                        .GetValues(typeof(Status))
                        .Cast<Status>()
                        .Select(s => new SelectListItem
                        {
                            Value = s.ToString(),
                            Text = s.GetDisplayName()
                        })
                        .ToList();

        //Requête de base
        var query = _context.Order
                            .Include(o => o.User)
                            .Include(o => o.Items)
                                .ThenInclude(i => i.Product)
                            .AsQueryable();

        //Filtre par statut optionnel
        if (FilterStatus.HasValue)
        {
            query = query.Where(o => o.Status == FilterStatus.Value);
        }

        // Chargement des commandes triées plus récentes en premier
        var orders = await query
                        .OrderByDescending(o => o.CreatedAt)
                        .ToListAsync();

        Orders = orders.Select(o => new OrderRowViewModel
        {
            Id = o.Id,
            //PublicId    = o.PublicId,
            CreatedAt = o.CreatedAt,
            Email = o?.User?.Email ?? "",   // adapte au besoin (ex: masquer une partie)
            ItemsCount = o?.Items?.Sum(i => i.Quantity) ?? 0,
            User = o.User,
            Total = o.PrixTTC,
            Status = o.Status
        }).ToList();

        // Compteurs par statut (sans tenir compte du filtre)
        CountPending = await _context.Order.CountAsync(o => o.Status == Status.Pending);
        CountCancelled = await _context.Order.CountAsync(o => o.Status == Status.Cancelled);
        CountCompleted = await _context.Order.CountAsync(o => o.Status == Status.Completed);
        CountDelivered = await _context.Order.CountAsync(o => o.Status == Status.Delivered);

    }

    public async Task<IActionResult> OnPostChangeStatusAsync(int orderId, Status status)
    {
        var order = await _context.Order.FindAsync(orderId);
        if (order == null) return RedirectToPage(new { FilterStatus });

        order.Status = status;
        await _context.SaveChangesAsync();

        return RedirectToPage(new { FilterStatus });
    }

    public async Task<IActionResult> OnGetExportCsvAsync()
    {
        var query = _context.Order
                    .Include(o => o.User)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .AsQueryable();

        if (FilterStatus.HasValue)
        {
            query = query.Where(o => o.Status == FilterStatus.Value);
        }

        var orders = await query
                    .OrderByDescending(o => o.CreatedAt)
                    .ToListAsync();

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("IdCommande;Date;Client;Articles;Total;Statut");

        foreach (var o in orders)
        {
            stringBuilder.AppendLine(
                $"{o.Id};" +
                $"{o.CreatedAt};" +
                $"{o.User.Email};" +
                $"{o.Items.Sum(i => i.Quantity)};" +
                /* $"{o.Items.Count()};" + */
                $"{o.TotalAmount};" +
                $"{o.Status.GetDisplayName()}"
            );
        }

        var bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        var bom = Encoding.UTF8.GetPreamble();
        var finalContent = bom.Concat(bytes).ToArray();
        var fileName = $"commandes_{DateTime.UtcNow::yyyyMMdd_HHmmss}.csv";
        return File(bytes, "text/csv", fileName);
    }

    public OrderDetailViewModel OrderDetailViewModel { get; set; } = new OrderDetailViewModel();
    public int? Id { get; set; } = default!;

    // SUPPRIME TOUT le reste, garde SEULEMENT ça :

    public async Task<IActionResult> OnGetOrderDetailsJsonAsync(int id)
    {
        var order = await _context.Order
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (order == null)
        {
            return NotFound(new { error = "Commande introuvable" });
        }

        var viewModel = new OrderDetailViewModel
        {
            Id = id,
            DateCommande           = order.CreatedAt,
            StatusCommande         = order.Status.GetDisplayName(),
            ModeLivraison          = "Mode express",
            PrixHT                 = order.PrixHT,
            PrixTotalCommande      = order.PrixTTC,
            AdresseCommande        = $"{order.Address}, {order.City}",
            orderItemRowViewModels = order.Items?.Select(oi => new OrderItemRowViewModel
            {
                NomProduit = oi?.Product?.Name ?? "Produit supprimé",
                Quantite   = oi?.Quantity ?? 0,
                PrixUnit   = oi?.UnitPrice ?? 0m,
                PrixTotal  = (oi?.UnitPrice ?? 0m) * (oi?.Quantity ?? 0),
                ImageUrl   = oi?.Product?.ImageUrl ?? "/img/placeholder-product.jpg"
            }).ToList() ?? new()
        };

        return new JsonResult(viewModel);
    }

}
