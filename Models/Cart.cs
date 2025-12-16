using System.ComponentModel.DataAnnotations;

namespace E_commerce_c_charp.Models;
public class Cart
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public User User { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
