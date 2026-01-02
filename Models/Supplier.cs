using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using E_commerce_c_charp.Models;


namespace E_commerce_c_charp.Models;
public class Supplier
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; } = String.Empty;

    [StringLength(100)]
    public string Country { get; set; } = String.Empty;

    [StringLength(100)]
    public string CulturalOrigin { get; set; } = String.Empty;

    [Phone]
    [StringLength(20)]
    public string ContactPhone { get; set; } = String.Empty;

    [EmailAddress]
    [StringLength(255)]
    public string ContactEmail { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    [Required]
    [StringLength(50)] // Stocke "Active" ou "Suspended"
    public string Status { get; set; } = "Active";

    [Url]
    public string Website { get; set; } = String.Empty;
    
    [Url]
    public string Facebook { get; set; } = String.Empty;
    
    [Url]
    public string Instagram { get; set; } = String.Empty;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relation : Un fournisseur a plusieurs produits
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
