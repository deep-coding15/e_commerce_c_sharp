using System.ComponentModel.DataAnnotations;

namespace E_commerce_c_charp.ViewModels;

public class CreateSupplierRequestViewModel
{
    [Required(ErrorMessage = "Nom complet obligatoire")]
    [StringLength(255)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nom entreprise obligatoire")]
    [StringLength(255, MinimumLength = 3)]
    [Display(Name = "Nom de l'entreprise")]
    public string OrganizationName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type d'activité obligatoire")]
    [Display(Name = "Type d'activité")]
    public string ActivityType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pays obligatoire")]
    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Origine culturelle")]
    public string? CulturalOrigin { get; set; }

    [Required(ErrorMessage = "Types de produits obligatoires")]
    [Display(Name = "Types de produits")]
    public string ProductTypes { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description obligatoire")]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Téléphone obligatoire")]
    [Phone(ErrorMessage = "Format téléphone invalide")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email obligatoire")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    public string Email { get; set; } = string.Empty;

    [Url(ErrorMessage = "URL invalide")]
    public string? Website { get; set; }

    [Url(ErrorMessage = "URL invalide")]
    public string? Facebook { get; set; }

    [Url(ErrorMessage = "URL invalide")]
    public string? Instagram { get; set; }
}
