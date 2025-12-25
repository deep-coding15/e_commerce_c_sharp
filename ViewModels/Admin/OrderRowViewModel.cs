using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.ViewModels.Admin;

public class OrderRowViewModel
{
    public int Id { get; set; }
    public string PublicId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Email { get; set; } = default!;
    public User User { get; set; } = default!;
    public int ItemsCount { get; set; }
    public decimal Total { get; set; }
    public Status Status { get; set; }
    //public string Status { get; set; } = default!;
}
