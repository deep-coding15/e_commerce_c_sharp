using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration; //pour utiliser Column(TypeName)

namespace E_commerce_c_charp.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Sku { get; set; } = string.Empty;
    
    [StringLength(100, MinimumLength = 3), Required]
    public string Name { get; set; } = null!;
    
    [StringLength(500, MinimumLength = 5), Required]
    public string Description { get; set; } = null!;
    
    // Ajoutez cette ligne pour définir 2 décimales et une précision totale de 18 chiffres
    [StringLength(255, MinimumLength = 5)]
    public string ImageUrl { get; set; } = null!;
    
    // Ajoutez cette ligne pour définir 2 décimales et une précision totale de 18 chiffres
    [Required, Range(0.01, double.MaxValue), DataType(DataType.Currency), Column(TypeName = "decimal(18,2)"), IntegerValidator(MinValue = 0)]
    public decimal Price { get; set; }
    
    [Display(Name = "Stock Quantity"), IntegerValidator(MinValue = 0), Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    // Optionnel : marque
    [StringLength(50)]
    public string? Brand { get; set; }

    // Basic category system
    public int CategoryId { get; set; }
    
    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), Required, StringLength(30)]
    public Category? Category { get; set; }

    public bool IsFeatured { get; set; }   // pour le badge "Vedette"
    
    // Optionnel : rating texte (ex: "4.5/5")
    [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$"), StringLength(30), Required]
    public string Rating { get; set; } = string.Empty;

    // Navigation vers les lignes de commande
    public List<OrderItem> OrderItems { get; set; } = new();
}
