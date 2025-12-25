using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace E_commerce_c_charp.Models;
public class Order
{
    public int Id { get; set; }
    
    public string OrderNumber { get; set; } //#00001

    [Display(Name = "User Id")]
    public string? UserId { get; set; }
    public User? User { get; set; }

    [Column(TypeName = "decimal(18,2)"), Display(Name = "Total Amount"), Range(1, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    public static decimal TVA = 0.2M;

    [Required, EnumDataType(typeof(Status), ErrorMessage = "Le status est invalide.")]
    public Status Status { get; set; } = Status.Pending;

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DisplayName("Order Date")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔹 Infos client (doivent matcher le ViewModel)
    public string FullName { get; set; } = "";
    public string Email    { get; set; } = "";
    public string Address  { get; set; } = "";
    public DateTime EstimatedDelivery { get; set; }
    public string City     { get; set; } = "";
    public string Phone    { get; set; } = "";

    // 🔹 Totaux détaillés (si tu veux les stocker)
    public decimal PrixHT  { get; set; }
    public decimal PrixTVA { get; set; }
    public decimal PrixTTC { get; set; }
    

    // 🔹 Lignes de commande
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public enum Status {
    [Display(Name = "Pending", Description   = "La commande est en attente de paiement.")]
    Pending,
    [Display(Name = "Cancelled", Description = "La commande a été annulée.")]   
    Cancelled,
    [Display(Name = "Completed", Description = "La commande a été payée.")]
    Completed,
    [Display(Name = "Delivered", Description = "La commande a été livrée.")]
    Delivered,
}
public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType()
                          .GetMember(value.ToString())
                          .FirstOrDefault();

        var attr   = member?
            .GetCustomAttribute<DisplayAttribute>();

        return attr?.Name ?? value.ToString();
    }
}
