using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace E_commerce_c_charp.Models;
public class CartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;

    public int Quantity { get; set; }
    [Range(1, double.MaxValue), DataType(DataType.Currency), Column(TypeName = "decimal(18,2)"), 
    DisplayName("Unit Price"), IntegerValidator(MinValue = 0)]
    public decimal UnitPrice { get; set; }
}
