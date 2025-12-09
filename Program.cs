using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<E_commerce_c_charpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."))
);


var app = builder.Build();

/**This is for seeding a database : the database will work with a minimum of items in it.*/
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirects HTTP requests to HTTPS
app.UseHttpsRedirection();

// Adds route matching to the middleware pipeline
app.UseRouting();

/* Authorizes a user to access secure resources. 
This app doesn't use authorization, therefore this line could be removed. */
app.UseAuthorization();

app.MapStaticAssets();

//Configures endpoint routing for Razor Pages.
app.MapRazorPages()
   .WithStaticAssets(); //Optimize the delivery of static assets in an app, such as HTML, CSS, images, and JavaScript. 

app.Run();
