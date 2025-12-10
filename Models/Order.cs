using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_c_charp.Models;
public class Order
{
    public int Id { get; set; }

    [Display(Name = "User Id")]
    public string UserId { get; set; } = null!; // Identity user
    [Column(TypeName = "decimal(18,2)"), Display(Name = "Total Amount"), Range(1, double.MaxValue)] // Ajoutez cette ligne pour définir 2 décimales et une précision totale de 18 chiffres
    public decimal TotalAmount { get; set; }
    [Required, EnumDataType(typeof(Status), ErrorMessage = "Le status est invalide.")]
    public Status Status { get; set; } = Status.pending;
    
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DisplayName("Order Date")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public enum Status
{
    [Display(Name = "Pending", Description = "La commande est en attente de paiement.")]
    pending,
    [Display(Name = "Cancelled", Description = "La commande a été annulée.")]   
    cancelled,
    [Display(Name = "Completed", Description = "La commande a été payée.")]
    completed,
    [Display(Name = "Delivered", Description = "La commande a été livrée.")]
    delivered,
}