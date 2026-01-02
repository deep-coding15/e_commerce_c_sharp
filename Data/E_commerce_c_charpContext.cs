using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace E_commerce_c_charp.Data;
public class E_commerce_c_charpContext : IdentityDbContext<User, IdentityRole, string>
{
    public E_commerce_c_charpContext(DbContextOptions<E_commerce_c_charpContext> options) : 
        base(options)
    {

    }
    public DbSet<Product> Product {get; set;} = default!;
    public DbSet<Order> Order { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<OrderItem> OrderItem { get; set; } = default!;
    public DbSet<Cart> Cart { get; set; } = default!;
    public DbSet<CartItem> CartItem { get; set; } = default!;
    public DbSet<E_commerce_c_charp.Models.SupplierRequest> SupplierRequest { get; set; } = default!;
    public DbSet<Supplier> Supplier { get; set; } = default!;
}