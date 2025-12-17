using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace E_commerce_c_charp.Services;

public class NoOpEmailSender : IEmailSender<User>, IEmailSender
{
    private readonly ILogger<NoOpEmailSender> _logger;
    public NoOpEmailSender(ILogger<NoOpEmailSender> logger)
    {
        _logger = logger;
    }

    // Interface IEmailSender (ANCIENNE - utilisée par RegisterModel)
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("=== EMAIL SIMULÉ (ancienne interface) ===");
        _logger.LogInformation("À: {Email}", email);
        _logger.LogInformation("Sujet: {Subject}", subject);
        _logger.LogInformation("Message: {Message}", htmlMessage);
        return Task.CompletedTask;
    }
    
    // Interface IEmailSender>User> (nouvelle - .NET 9.0)
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        _logger.LogInformation("=== EMAIL DE CONFIRMATION ===");
        _logger.LogInformation("Utilisateur: {Username}", user.UserName);
        _logger.LogInformation("Email: {Email}", email);
        _logger.LogInformation("Lien: {Link}", confirmationLink);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        _logger.LogInformation("=== RÉINITIALISATION MOT DE PASSE ===");
        _logger.LogInformation("Utilisateur: {UserName}", user.UserName);
        _logger.LogInformation("Email: {Email}", email);
        _logger.LogInformation("Lien: {Link}", resetLink);
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        _logger.LogInformation("=== CODE DE RÉINITIALISATION ===");
        _logger.LogInformation("Utilisateur: {UserName}", user.UserName);
        _logger.LogInformation("Email: {Email}", email);
        _logger.LogInformation("Code: {Code}", resetCode);
        return Task.CompletedTask;
    }
}