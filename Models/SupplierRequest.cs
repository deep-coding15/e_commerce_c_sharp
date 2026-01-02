using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace E_commerce_c_charp.Models;

public class SupplierRequest
{
    public int Id { get; set; }

    [Required]
    [StringLength(255)] // Ajustez la taille selon vos besoins
    public string? FullName { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Display(Name = "Nom de l'entreprise")]
    public string OrganizationName { get; set; } = null!;

    [Required]
    [Display(Name = "Type d'activité")]
    public string ActivityType { get; set; } = String.Empty;

    [StringLength(100, MinimumLength = 5), Required]
    public string Country { get; set; } = null!;

    [StringLength(255, MinimumLength = 5)]
    public string CulturalOrigin { get; set; } = null!;

    [Required,]
    public decimal ProductTypes { get; set; }

    public string Description { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string Phone { get; set; } = String.Empty;

    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = String.Empty;

    [Url]
    public string Website { get; set; } = String.Empty;
    
    [Url]
    public string Facebook { get; set; } = String.Empty;
    
    [Url]
    public string Instagram { get; set; } = String.Empty;

    public bool IsReviewed { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
