using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E_commerce_c_charp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;
    public int? StatusCode { get; set; }

    /* public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    } */

    public void OnGet(int? statusCode)
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        // Récupère le code passé dans l'URL (/Error/404) 
        // ou celui envoyé par le middleware de réexécution
        StatusCode = statusCode;
    }
}
