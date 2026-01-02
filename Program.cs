// ====================================================================================================
// IMPORTS / USING STATEMENTS - Importe les bibliothèques nécessaires
// ====================================================================================================
using Microsoft.EntityFrameworkCore; // Pour Entity Framework Core (ORM pour la base de données)
using Microsoft.Extensions.DependencyInjection; // Pour l'injection de dépendances
using Microsoft.Extensions.Options; // Pour la configuration des options
using E_commerce_c_charp.Data; // Namespace contenant le contexte de base de données
using E_commerce_c_charp.Services; // Namespace contenant les services personnalisés
using E_commerce_c_charp.Models; // Namespace contenant les modèles (User, Product, etc.)
using Microsoft.AspNetCore.Identity; // Système d'authentification et d'autorisation ASP.NET Core
using Microsoft.AspNetCore.Identity.UI.Services; // Interface IEmailSender pour les emails Identity
using NSwag.AspNetCore; // Pour générer la documentation Swagger/OpenAPI
using Microsoft.AspNetCore.Mvc; // Contrôleurs MVC et résultats HTTP
using Microsoft.AspNetCore.Antiforgery; // Protection CSRF (Cross-Site Request Forgery)
using E_commerce_c_charp.Models.Requests; // Modèles pour les requêtes API
using System.Reflection.Metadata; // Métadonnées .NET (peut-être pour la réflexion)
using E_commerce_c_charp.EndPoints; // Endpoints minimal API personnalisés
using AutoMapper; // Bibliothèque pour mapper les objets (DTO → Entity)
using E_commerce_c_charp.Mapping; // Profils AutoMapper personnalisés
using Serilog;
using Serilog.AspNetCore;

// ====================================================================================================
// CRÉATION DE L'APPLICATION - Point d'entrée principal
// ====================================================================================================
var builder = WebApplication.CreateBuilder(args); // Crée le constructeur d'application web

// ====================================================================================================
// SERVICES - Enregistrement des services dans le conteneur DI (Dependency Injection)
// ====================================================================================================

// 1. RAZOR PAGES - Active les pages Razor (pages .cshtml avec code-behind)
builder.Services.AddRazorPages(options =>
{
    // Sécurise tout le dossier Admin pour le rôle "Admin"
    options.Conventions.AuthorizeFolder("/Admin", "RequireAdminRole");
}); // Permet d'utiliser les pages comme /Product/Index.cshtml

// 2. BASE DE DONNÉES - Configuration du contexte EF Core
builder.Services.AddDbContext<E_commerce_c_charpContext>(options =>
    // Configure le contexte pour utiliser SQL Server
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection") // Récupère la chaîne de connexion depuis appsettings.json
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))); // Erreur si pas trouvée

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<E_commerce_c_charpContext>();



// 3. FILTRE D'EXCEPTIONS DB - UNIQUEMENT EN DÉVELOPPEMENT
/* Intercepte les exceptions liées à Entity Framework Core uniquement 
   lorsque l'application tourne en mode Développement. Affiche une page 
   avec "Apply Migrations" pour réparer facilement les erreurs DB */
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 4. PROTECTION CSRF - Sécurité contre les attaques CSRF
builder.Services.AddAntiforgery(
    options =>
    {
        options.HeaderName = "X-CSRF-TOKEN"; // Nom de l'en-tête HTTP utilisé pour les tokens CSRF
        // Les formulaires enverront ce token dans l'en-tête pour validation
    });

// ====================================================================================================
// IDENTITY - Système d'authentification complet
// ====================================================================================================
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // RÈGLES DE MOT DE PASSE - Personnalisation des exigences
    options.Password.RequiredLength = 6; // Minimum 6 caractères
    options.Password.RequireDigit = true; // Au moins 1 chiffre obligatoire
    options.Password.RequireUppercase = false; // PAS de majuscule obligatoire
    options.Password.RequireNonAlphanumeric = false; // PAS de caractère spécial obligatoire

    // Confirmation du compte par email (IMPORTANT pour ForgotPassword)
    options.SignIn.RequireConfirmedAccount = true; // L'utilisateur doit confirmer son email
})
.AddEntityFrameworkStores<E_commerce_c_charpContext>() // Stocke les users/roles dans la DB EF Core
.AddDefaultTokenProviders() // Génère les tokens pour reset password, confirmation email, etc.
.AddRoles<IdentityRole>(); // Ajoute la gestion des rôles

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});

// 5. COOKIES D'AUTHENTIFICATION - Chemins des pages Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Redirige ici si non connecté
    options.LogoutPath = "/Identity/Account/Logout"; // Page de déconnexion
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Accès refusé
});

// 6. SERVICES EMAIL - CRITIQUE POUR FORGOT PASSWORD
//builder.Services.AddScoped<IEmailSender, EmailSender>(); // Version réelle (commentée)
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
// Deuxième enregistrement pour la version générique avec User
builder.Services.AddTransient<IEmailSender<User>, SmtpEmailSender>();

// builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// 7. SESSIONS - Stockage temporaire de données (panier par exemple)
builder.Services.AddSession(); // Active les sessions utilisateur

// 8. DOCUMENTATION API - Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer(); // Explore les endpoints pour la doc
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "E commerce"; // Nom du document Swagger
    config.Title = "E commerce v1"; // Titre affiché
    config.Version = "v1"; // Version de l'API
});

// 9. AUTOMAPPER - Mapping automatique des objets
builder.Services.AddAutoMapper(config =>
{
    // Ajoute le profil de mapping spécifique aux commandes
    config.AddProfile<E_commerce_c_charp.Mapping.ProfileOrder>();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//Active la journalisation
builder.Logging.AddConsole();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
        theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Code
    ).WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning
    ).CreateLogger();

builder.Logging.ClearProviders();
//builder.Logging.AddSerilog();
builder.Services.AddSerilog();

// ====================================================================================================
// CONSTRUCTION ET INITIALISATION
// ====================================================================================================
var app = builder.Build(); // Construit l'application finale

// SEEDING - Initialise la base de données avec des données de test
/** Crée un scope temporaire pour accéder aux services et initialiser la DB */
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider; // Récupère le provider de services
    try
    {
        // Appelle la méthode d'initialisation
        await SeedData.Initialize(services);
    }
    catch (Exception exception)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "Une erreur est survenue lors du seeding de la base de données.");
    }
}

// ====================================================================================================
// MIDDLEWARE PIPELINE - Ordre CRITIQUE (chaque middleware traite la requête dans l'ordre)
// ====================================================================================================

// 10. GESTION DES ERREURS - Différent selon l'environnement
if (!app.Environment.IsDevelopment()) // Production
{
    app.UseExceptionHandler("/Error"); // Page d'erreur générique
    app.UseHsts(); // Sécurité HTTPS stricte (30 jours par défaut)
}
else // Développement
{
    app.UseDeveloperExceptionPage(); // Affiche les détails complets des erreurs
    /* Page pour gérer les migrations DB automatiquement */
    app.UseMigrationsEndPoint();

    // Swagger UNIQUEMENT en développement
    app.UseOpenApi(); // Génère les fichiers OpenAPI
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "E commerce App"; // Titre Swagger
        config.Path = "/swagger"; // URL d'accès : /swagger
        config.DocumentPath = "/swagger/{documentName}/swagger.json"; // Chemin JSON
        config.DocExpansion = "list"; // État par défaut des sections
    });
}

// Gère les erreurs HTTP (404, 403, etc.) en réexécutant le pipeline vers une route personnalisée.
// Le jeton {0} est remplacé par le code d'erreur tout en conservant l'URL d'origine dans le navigateur.
app.UseStatusCodePagesWithReExecute("/Error/{0}");

// 11. SÉCURITÉ HTTPS
app.UseHttpsRedirection(); // Force HTTPS (redirige http → https)

// 12. ROUTING - Associe les URLs aux contrôleurs/pages
app.UseRouting(); // **ORDRE IMPORTANT** avant Authentication

// 13. AUTHENTIFICATION - Vérifie l'identité de l'utilisateur
app.UseAuthentication(); // **DOIT être AVANT UseAuthorization**

// 14. AUTORISATION - Vérifie les permissions
app.UseAuthorization(); // **DOIT être APRÈS UseAuthentication**

// 15. SESSIONS - Active après le routage
app.UseSession(); // **ORDRE IMPORTANT**

// 16. CSRF - Protection après authentification
app.UseAntiforgery(); // Valide les tokens CSRF

// 17. FICHIERS STATIQUES
app.MapStaticAssets(); // CSS, JS, images (wwwroot)

// 18. RAZOR PAGES - Routage des pages .cshtml
app.MapRazorPages()
   .WithStaticAssets(); // Optimise les fichiers statiques

// ====================================================================================================
// ENDPOINTS PERSONNALISES - Redirections et API
// ====================================================================================================

// Page d'accueil :
// - Client : Liste des produits
// - Admin  : Dashbaord Application

app.MapGet("/", (HttpContext context) =>
{
    var user = context.User;

    // Non connecté => page client par défaut
    if (user?.Identity is null || !user.Identity.IsAuthenticated)
    {
        return Results.Redirect("/Product");
    }

    // Admin
    if (user.IsInRole("Admin"))
    {
        return Results.Redirect("/Admin/Dashboard");
    }

    // Client normal
    return Results.Redirect("/Product");
});

// API TOKEN CSRF - Génère et stocke le token dans un cookie
app.MapGet("/antiforgery/token", (IAntiforgery antiForgery, HttpContext context) =>
{
    var tokens = antiForgery.GetAndStoreTokens(context); // Génère les tokens
    // Stocke le token dans un cookie accessible au JavaScript (HttpOnly=false)
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
    new CookieOptions { HttpOnly = false }); // JavaScript peut le lire
    return Results.Ok(); // Réponse 200 OK
});

// Redirections API générales
app.MapGet("/api", () => Results.Redirect("/Product/Index"));
app.MapGet("/Checkout", () => Results.Redirect("/Checkout/Index"));
app.MapGet("/Catalogue", () => Results.Redirect("/Catalogue/Index"));

// GROUPE PRODUITS - /api/Product
var productItems = app.MapGroup("/api/Product");
productItems.MapGet("Details/{id:int}", (int id) => Results.Redirect($"/Product/Details?id={id}"));
productItems.MapGet("", () => Results.Redirect("/Product/Index"));

// GROUPE API PRINCIPAL
var apiItems = app.MapGroup("/api");
var orderItems = apiItems.MapGroup("/Order"); // /api/Order
orderItems.MapGet("Details/{id:int}", (int id) => Results.Redirect($"/Order/Details?id={id}"));

// ENDPOINTS PERSONNALISÉS - Méthodes extension dans d'autres fichiers
app.MapCartEndpoints();     // Panier (pages Razor)
app.MapCartEndpointsApi();  // Panier (API JSON)
app.MapOrderEndpointsApi(); // Commandes (API JSON)

app.MapGet("/forgotpasswordconfirmation", () =>
{
    var html = @"
        <h1>Forgot password confirmation</h1>
        <p>Please check your email to reset your password.</p>";
    return Results.Content(html, "text/html");
});

app.MapGet("/test-email", async (IEmailSender emailSender) =>
{
    try
    {
        await emailSender.SendEmailAsync(
            email: "tsafackmerveille15@gmail.com",
            subject: "Test email",
            htmlMessage: "<h1>Ceci est un test d'email</h1><p>Si vous voyez ce message, l'envoi d'email fonctionne !</p>"
        );
        return Results.Ok("Email envoyé avec succès !");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Erreur lors de l'envoi de l'email : {ex.Message}");
    }
});

/* app.MapGet("/test-forgotpassword", async (IEmailSender<User> emailSender) =>
{
    try
    {
        var user = new User { UserName = "Test" };

        await emailSender.SendPasswordResetLinkAsync(
            user: user,
            email: "tsafackmerveille15@gmail.com"//, 
            //resetLink: "http://localhost:5283/Identity/Account/ForgotPasswordConfirmation"
        );

        return Results.Ok("Email de réinitialisation envoyé avec succès !");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Erreur lors de l'envoi de l'email : {ex.Message}");
    }
});
 */

// ====================================================================================================
// DÉMARRAGE
// ====================================================================================================
app.Run(); // Lance le serveur Kestrel
