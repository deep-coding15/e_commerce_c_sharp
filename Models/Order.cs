using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_c_charp.Models;
public class Order
{
    public int Id { get; set; }

    [Display(Name = "User Id")]
    public string UserId { get; set; } = null!; // Identity user
    [Column(TypeName = "decimal(18,2)")] // Ajoutez cette ligne pour définir 2 décimales et une précision totale de 18 chiffres
    [Display(Name = "Total Amount")]
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "pending";
    
    [DataType(DataType.DateTime)]
    [DisplayName("Order Date")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
