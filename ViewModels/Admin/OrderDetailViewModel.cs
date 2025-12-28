using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.ViewModels.Admin;
public class OrderDetailViewModel
{
    public int Id { get; set; } = default!;
    public string OrderNumber { get; set; } = default!;
    public DateTime DateCommande { get; set; } = default!;
    public string ModeLivraison { get; set; } = "Livraison Normal";
    public string StatusCommande { get; set; } = default!;
    public decimal PrixHT { get; set; } = default!;
    public decimal PrixTotalCommande { get; set; } = default!;
    public string AdresseCommande { get; set; } = default!;

    public IList<OrderItemRowViewModel> orderItemRowViewModels { get; set; } = default!;
}

public class OrderItemRowViewModel
{
    public string NomProduit { get; set; } = default!;
    public int Quantite { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public decimal PrixUnit { get; set; } = default!;
    public decimal PrixTotal { get; set; } = default!;
}