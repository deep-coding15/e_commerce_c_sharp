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

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), StringLength(30)]
    public Category Category { get; set; } = default!;

    public bool IsFeatured { get; set; }   // pour le badge "Vedette"
    public bool IsArchived { get; set; } = false;

    // Optionnel : rating texte (ex: "4.5/5")
    [Range(0.0, 5.0, ErrorMessage = "La note doit être entre 0.0 et 5.0")]
    [RegularExpression(@"^[0-5](\.[0-9]{1,2})?$", ErrorMessage = "Format : 4.5, 3.2, 5")]
    [Required(ErrorMessage = "La note est obligatoire")]
    [Column(TypeName = "decimal(3,2)")] // 4.50 max
    public decimal Rating { get; set; } = 0.0m;

    // Navigation vers les lignes de commande
    public List<OrderItem> OrderItems { get; set; } = new();
    
    // Clé étrangère et relation vers le fournisseur
    
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}
