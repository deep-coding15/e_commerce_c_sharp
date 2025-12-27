using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.ViewModels.Admin;
public class DashboardViewModel
{
    // Cartes KPI
    /// <summary>
    /// Nombre total des commandes meme ceux en attente et annulés
    /// </summary>
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }

    public decimal Revenue { get; set; }
    public decimal AvgOrder { get; set; }

    public int ProductsCount { get; set; }
    public int UsersCount { get; set; }

    // Graphique ligne : tendance des ventes
    // ex: mois = ["Jan", "Feb", ...], valeurs = [7000, 8000, ...]
    public List<string> SalesMonths { get; set; } = new();
    public List<decimal> SalesTrend { get; set; } = new();

    // Graphique camembert : statuts des commandes
    // clé = "Livrée", "Confirmée", "En préparation", "Expédiée"
    public Dictionary<string, int> OrderStatusCounts { get; set; } = new();

    // Produits populaires
    public List<TopProductViewModel> TopProducts { get; set; } = new();

    // Commandes récentes
    public List<RecentOrderViewModel> RecentOrders { get; set; } = new();
}

public class TopProductViewModel
{
    public int Rank { get; set; }          // #1, #2, ...
    public string Name { get; set; } = "";
    public int SoldCount { get; set; }     // nb ventes
    public decimal Price { get; set; }     // prix affiché
}

public class RecentOrderViewModel
{
    public string Number { get; set; } = "";   // ex: "#8595D0C"
    public DateTime Date { get; set; }        // date commande
    public decimal Total { get; set; }        // montant
    public string StatusLabel { get; set; } = ""; // "Expédiée", "Confirmée", ...
}
