namespace E_commerce_c_charp.ViewModels;

public class OrderSummaryViewModel
{
    public string OrderNumber { get; set; } = "";              // ex: #855f9f0c-642
    public DateTime CreatedAt { get; set; }
    public int ItemsCount { get; set; }
    public string ShippingLabel { get; set; } = "Livraison express";
    public string StatusLabel { get; set; } = "Confirmee"; // Commandée / Confirmée / En préparation / Expédiée / Livrée
    public string StatusCssClass { get; set; } = "bg-primary"; // badge couleur
    public string DeliveryAddress { get; set; } = "";
    public DateTime EstimatedDelivery { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new();
}