using E_commerce_c_charp.Models;
namespace E_commerce_c_charp.ViewModels.Admin;

public class OrderItemViewModel
{
    /// <summary>
    /// string: Nom de la catégorie
    /// int   : Nombre de produit par catégorie
    /// </summary>
    public Dictionary<String, int>? VentesParCategory { get; set; }
    public int NombreCommandesLivrees {get; set; }
    public decimal PrixMoyenDesCommandes { get; set; }
    public decimal NombreProduitsEnStock { get; set; }
    public decimal NombreProduitsEnFaibleStock { get; set; }
    public decimal NombreProduitsEnRuptureDeStock { get; set; }
}