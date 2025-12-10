using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_commerce_c_charp.Models;
public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }
    [Range(0, int.MaxValue), Required]
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18,2)"), Range(0, int.MaxValue), Required] // Ajoutez cette ligne pour définir 2 décimales et une précision totale de 18 chiffres
    public decimal UnitPrice { get; set; }
}
