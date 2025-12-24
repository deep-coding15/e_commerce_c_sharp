// ViewModels/OrderItemViewModel.cs
namespace E_commerce_c_charp.ViewModels;

public class OrderItemViewModel
{
    public string ProductName { get; set; } = "";
    public string? Variant { get; set; }              // Taille, couleur, etc.
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string ImageUrl { get; set; } = "";
}
