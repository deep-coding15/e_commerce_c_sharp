using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using E_commerce_c_charp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_c_charp.ViewModels.Admin;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Le SKU est obligatoire")]
    [StringLength(50, ErrorMessage = "Le SKU ne peut pas dépasser 50 caractères")]
    [Display(Name = "SKU")]
    public string Sku { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Le nom doit faire entre 3 et 100 caractères")]
    [Display(Name = "Nom du produit")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est obligatoire")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "La description doit faire entre 5 et 500 caractères")]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "L'URL de l'image ne peut pas dépasser 255 caractères")]
    [Url(ErrorMessage = "URL d'image invalide")]
    [Display(Name = "URL Image")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Le prix est obligatoire")]
    [Range(0.01, 99999.99, ErrorMessage = "Le prix doit être entre 0.01€ et 99 999€")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Prix (€)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Le stock est obligatoire")]
    [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif")]
    [Display(Name = "Stock")]
    public int StockQuantity { get; set; }

    [StringLength(50, ErrorMessage = "La marque ne peut pas dépasser 50 caractères")]
    [RegularExpression(@"^[A-Za-z\s\-&']+$", ErrorMessage = "Caractères alphanumériques uniquement")]
    [Display(Name = "Marque")]
    public string? Brand { get; set; }

    [Required(ErrorMessage = "La catégorie est obligatoire")]
    [Display(Name = "Catégorie")]
    public int CategoryId { get; set; }
    //public Category Category { get; set; } = default!;
    public IEnumerable<SelectListItem> Category { get; set; } = Enumerable.Empty<SelectListItem>();
    [Required(ErrorMessage = "La note est obligatoire")]
    [Range(0.0, 5.0, ErrorMessage = "La note doit être entre 0.0 et 5.0")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Format valide: 4.5, 3.2, 5.0")]
    [Display(Name = "Note (/5)")]
    public decimal Rating { get; set; } = 0.0m;

    [Display(Name = "Fournisseur")]
    public int? SupplierId { get; set; }
    public IEnumerable<SelectListItem> Supplier { get; set; } = Enumerable.Empty<SelectListItem>();
    //public Supplier Supplier { get; set; } = default!;

    [Display(Name = "Produit vedette")]
    public bool IsFeatured { get; set; }

    // Select lists
    public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Suppliers { get; set; } = Enumerable.Empty<SelectListItem>();
}
