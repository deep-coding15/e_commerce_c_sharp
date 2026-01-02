using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Models;
public class User : IdentityUser
{
    [Required, MinLength(3), StringLength(50)]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s'-]+$", ErrorMessage = "Nom invalide")]
    public string Nom { get; set; } = null!;
    
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /* public int CartId {get; set;} */
    
    public IList<Cart> Cart { get; set; } = default!; // Propriété de navigation vers le panier
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
