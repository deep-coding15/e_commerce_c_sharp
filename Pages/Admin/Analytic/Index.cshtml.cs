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

namespace E_commerce_c_charp.Pages_Admin_OrderItem
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly ILogger<User> _logger;

        public IndexModel(
            E_commerce_c_charpContext context, 
            ILogger<User> logger
        )
        {
            _context = context;
            _logger      = logger;
        }

        public OrderItemViewModel? OrdersItem { get;set; } = new OrderItemViewModel();

        public async Task OnGetAsync()
        {
            var ventesParCategory = await _context.OrderItem
                .Include(o => o.Product)
                    .ThenInclude(p => p.Category)
                .Where(o => o.Product.Category != null)
                .GroupBy(o => o.Product.Category)
                .ToDictionaryAsync(g => g.Key.Name, g => g.Sum(o => o.Quantity));

            var nombreCommandesLivrees = _context.OrderItem
                                            .Include(oi => oi.Order)
                                            .Where(oi => oi.Order.Status == Status.Completed)
                                            .Count();
            //OrderItem = null;

            var prixMoyenDesCommandes = await _context.Order
                                                .Select(o => (decimal?)o.PrixTTC)
                                                .AverageAsync() ?? 0m;                                                                                         

            var nombreProduitsEnStock = _context.Product
                .Where(p => p.StockQuantity >= 20)
                .Count();

            var nombreProduitsEnFaibleStock = _context.Product
                .Where(p => p.StockQuantity < 20)
                .Count();

            var nombreProduitsEnRuptureDeStock = _context.Product
                .Where(p => p.StockQuantity <= 0)
                .Count();

            
            OrdersItem.VentesParCategory              = ventesParCategory;
            OrdersItem.PrixMoyenDesCommandes          = prixMoyenDesCommandes;
            OrdersItem.NombreCommandesLivrees         = nombreCommandesLivrees;
            
            OrdersItem.NombreProduitsEnStock          = nombreProduitsEnStock;
            OrdersItem.NombreProduitsEnFaibleStock    = nombreProduitsEnFaibleStock;
            OrdersItem.NombreProduitsEnRuptureDeStock = nombreProduitsEnRuptureDeStock;

            _logger.LogWarning($"order: {ventesParCategory.ToJson()}");
            _logger.LogWarning($"nb commande livrees: {nombreCommandesLivrees.ToJson()}");
        
            _logger.LogWarning($"prix moyen commande: {prixMoyenDesCommandes.ToJson()}");
            _logger.LogWarning($"nb produits en stock: {nombreProduitsEnStock.ToJson()}");
        
            _logger.LogWarning($"nb produits en faible stock: {nombreProduitsEnFaibleStock.ToJson()}");
            _logger.LogWarning($"nb produits en rupture de stock: {nombreProduitsEnRuptureDeStock.ToJson()}");
        }
    }
}
