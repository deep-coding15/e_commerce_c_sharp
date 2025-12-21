using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Services;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using E_commerce_c_charp.Models.Requests;
using System.Reflection.Metadata;
using E_commerce_c_charp.EndPoints;
using AutoMapper;
using E_commerce_c_charp.Mapping;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<E_commerce_c_charpContext>(options =>
    options.UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))
);

/* intercepte les exceptions liées à Entity Framework Core uniquement 
    lorsque l'application tourne en mode Développement. */
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAntiforgery(
    options =>
    {
        options.HeaderName = "X-CSRF-TOKEN"; // Use a common header name
    }
);


// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedAccount = false; // Désactive la confirmation
})
.AddEntityFrameworkStores<E_commerce_c_charpContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddTransient<IEmailSender, E_commerce_c_charp.Services.NoOpEmailSender>();
builder.Services.AddTransient<IEmailSender<User>, E_commerce_c_charp.Services.NoOpEmailSender>();

builder.Services.AddSession();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "E commerce";
    config.Title = "E commerce v1";
    config.Version = "v1";
});

// Cela permet ensuite d’injecter IMapper dans les PageModel.
builder.Services.AddAutoMapper(config =>
{
    // Ici on ajoute  les profils
    config.AddProfile<E_commerce_c_charp.Mapping.ProfileOrder>();
}); 
//builder.Services.AddAutoMapper(typeof(ProfileOrder)); 

var app = builder.Build();
//var userManager = ServiceProvider
/**This is for seeding a database : the database will work with a minimum of items in it.*/
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Middleware

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage(); // Affiche l'erreur détaillée
    /* Nécessite le package Diagnostics :  
        Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore */
    app.UseMigrationsEndPoint();     // Permet de cliquer sur "Apply Migrations" pour réparer l'erreur

    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "E commerce App";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();

// Adds route matching to the middleware pipeline
app.UseRouting();

app.UseAuthentication();
/* Authorizes a user to access secure resources. 
This app doesn't use authorization, therefore this line could be removed. */
app.UseAuthorization();

app.UseSession(); // après UseRouting

app.UseAntiforgery();

app.MapStaticAssets();

//Configures endpoint routing for Razor Pages.
app.MapRazorPages()
   .WithStaticAssets(); //Optimize the delivery of static assets in an app, such as HTML, CSS, images, and JavaScript. 

// Redirection de la page d'accueil vers les produits
app.MapGet("/", () => Results.Redirect("/Product/Index"));

app.MapGet("/antiforgery/token", (IAntiforgery antiForgery, HttpContext context) =>
{
    var tokens = antiForgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
    new CookieOptions { HttpOnly = false });
    return Results.Ok();
});

app.MapGet("/api", () => Results.Redirect("/Product/Index"));
app.MapGet("/Checkout", () => Results.Redirect("/Checkout/Index"));

var productItems = app.MapGroup("/api/Product");
productItems.MapGet("Details/{id:int}", (int id) => Results.Redirect($"/Product/Details?id={id}"));
productItems.MapGet("", () => Results.Redirect("/Product/Index"));

//app.MapGet("/Cart", () => Results.Redirect("/Cart/Index"));

var apiItems = app.MapGroup("/api");
var orderItems = apiItems.MapGroup("/Order");
orderItems.MapGet("Details/{id:int}", (int id) => Results.Redirect($"/Order/Details?id={id}"));
//orderItems.MapGet("", () => Results.Redirect("/Order/Index"));

app.MapCartEndpoints();
app.MapCartEndpointsApi();
app.MapOrderEndpointsApi();

/* app.MapGet('', async () => await );
app.MapPost('', async () => await );
app.MapPut('', async () => await );
app.MapDelete('', async () => await ); */

app.Run();
