using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; //pour utiliser Column(TypeName)

namespace E_commerce_c_charp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")] // Ajoutez cette ligne pour définir 2 décimales et une précision totale de 18 chiffres
    public decimal Price { get; set; }
    [Display(Name = "Stock Quantity")]
    public int StockQuantity { get; set; }

    // Basic category system
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public string Rating { get; set; } = string.Empty;
}
