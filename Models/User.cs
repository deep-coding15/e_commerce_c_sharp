using System.ComponentModel.DataAnnotations;

namespace E_commerce_c_charp.Models;
public class User
{
    public int Id { get; set; }
    [DataType(DataType.EmailAddress)]
    [Required, StringLength(100)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        ErrorMessage = "Adresse email invalide")
    ]
    public string Email { get; set; } = null!;
    [Required, StringLength(255)]
    public string PasswordHash { get; set; } = null!;
    [Required, MinLength(3), StringLength(50)]
    [RegularExpression(@"^[a-zA-ZÀ-ÿ\s'-]+$", ErrorMessage = "Nom invalide")]
    public string Nom { get; set; } = null!;
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /* public int CartId {get; set;} */
    public Cart? Cart { get; set; } // Propriété de navigation vers le panier
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
