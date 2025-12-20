using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.ViewModels;
public class CartItemViewModel
{
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Quantity * (Product?.Price ?? 0);
}