// ViewModels/OrderDashboardViewModel.cs
namespace E_commerce_c_charp.ViewModels;

public class OrderDashboardViewModel
{
    public decimal TotalSpent { get; set; }
    public int InProgress { get; set; }
    public int Delivered { get; set; }
    public int ItemsPurchased { get; set; }
    public int OrdersCount { get; set; }

    public List<OrderSummaryViewModel> Orders { get; set; } = new();
}
