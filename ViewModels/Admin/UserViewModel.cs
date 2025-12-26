using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.ViewModels.Admin;

public class UserViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Role { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; }
    public int OrdersCount { get; set; }
}