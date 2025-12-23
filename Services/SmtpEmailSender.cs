using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace E_commerce_c_charp.Services
{
    /// <summary>
    /// Envoi réel d'e-mails via SMTP
    /// Implémente les deux interfaces : IEmailSender (simple) ET IEmailSender<User> (avancée)
    /// </summary>
    public class SmtpEmailSender : IEmailSender<User>, IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ===============================================
        // MÉTHODES IEmailSender (SIMPLE) - Déjà présente
        // ===============================================
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Création du message
            var message = new MailMessage
            {
                From = new MailAddress(
                    _configuration["Email:Smtp:From"]
                ),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            // Email destinataire
            message.To.Add(email);

            // Configuration du client SMTP
            var client = new SmtpClient
            {
                Host = _configuration["Email:Smtp:Host"],
                Port = int.Parse(_configuration["Email:Smtp:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["Email:Smtp:Username"],
                    _configuration["Email:Smtp:Password"]
                )
            };

            await client.SendMailAsync(message);
        }

        // ===============================================
        // MÉTHODES IEmailSender<User> (AVANCÉES)
        // ===============================================

        /// <summary>
        /// Envoie le lien de confirmation de compte
        /// Utilisé quand un utilisateur s'inscrit
        /// </summary>
        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            var subject = "Confirmez votre compte E-commerce";
            var htmlMessage = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #007bff;'>Bienvenue {user.UserName} !</h2>
                    <p>Merci de vous être inscrit sur notre plateforme E-commerce.</p>
                    <p>Pour activer votre compte, cliquez sur le bouton ci-dessous :</p>
                    <a href='{confirmationLink}' 
                       style='background-color: #007bff; color: white; padding: 12px 24px; 
                              text-decoration: none; border-radius: 5px; display: inline-block;'>
                        Confirmer mon compte
                    </a>
                    <p><small>Si le bouton ne fonctionne pas, copiez ce lien : <br>{confirmationLink}</small></p>
                    <hr>
                    <p>À bientôt sur E-commerce !</p>
                </div>";

            await SendEmailAsync(email, subject, htmlMessage);
        }

        /// <summary>
        /// Envoie le lien de réinitialisation de mot de passe
        /// Utilisé par la page ForgotPassword
        /// </summary>
        public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            var subject = "Réinitialisez votre mot de passe - E-commerce";
            var htmlMessage = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #dc3545;'>Réinitialisation de mot de passe</h2>
                    <p>Bonjour {user.UserName},</p>
                    <p>Vous avez demandé à réinitialiser votre mot de passe.</p>
                    <p>Cliquez sur le bouton ci-dessous pour créer un nouveau mot de passe :</p>
                    <a href='{resetLink}' 
                       style='background-color: #dc3545; color: white; padding: 12px 24px; 
                              text-decoration: none; border-radius: 5px; display: inline-block;'>
                        Réinitialiser mon mot de passe
                    </a>
                    <p><small>Ce lien expire dans 1 heure. Si vous n'avez pas demandé cela, ignorez cet email.</small></p>
                    <hr>
                    <p>L'équipe E-commerce</p>
                </div>";

            await SendEmailAsync(email, subject, htmlMessage);
        }

        /// <summary>
        /// Envoie un code de réinitialisation par email (alternative au lien)
        /// </summary>
        public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            var subject = "Votre code de réinitialisation - E-commerce";
            var htmlMessage = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #28a745;'>Votre code de réinitialisation</h2>
                    <p>Bonjour {user.UserName},</p>
                    <p>Utilisez ce code pour réinitialiser votre mot de passe :</p>
                    <div style='background-color: #f8f9fa; padding: 20px; text-align: center; 
                                font-size: 24px; font-weight: bold; color: #28a745; 
                                letter-spacing: 5px; border: 2px solid #28a745; 
                                border-radius: 10px; max-width: 300px; margin: 20px auto;'>
                        {resetCode}
                    </div>
                    <p><small>Ce code expire dans 1 heure.</small></p>
                    <hr>
                    <p>L'équipe E-commerce</p>
                </div>";

            await SendEmailAsync(email, subject, htmlMessage);
        }
    }
}
