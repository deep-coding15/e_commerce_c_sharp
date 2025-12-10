using System.ComponentModel.DataAnnotations;

namespace E_commerce_c_charp.Models;
public class Category
{
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 3), Required]
    public string Name { get; set; } = null!;

    public ICollection<Product>? Products { get; set; }
}