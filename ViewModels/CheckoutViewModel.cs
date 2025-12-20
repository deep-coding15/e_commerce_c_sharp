namespace E_commerce_c_charp.ViewModels;

public class CheckoutViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    public decimal PrixHT { get; set; }
    public decimal PrixTVA { get; set; }
    public decimal PrixTTC { get; set; }

    // Infos client
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string Phone { get; set; } = "";
}
