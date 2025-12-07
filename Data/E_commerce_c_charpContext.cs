using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Models;

namespace E_commerce_c_charp.Data;
public class E_commerce_c_charpContext : DbContext
{
    public E_commerce_c_charpContext(DbContextOptions<E_commerce_c_charpContext> options) : base(options){

    }
    public DbSet<Product> Product {get; set;} = default!;
    public DbSet<Order> Order { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<OrderItem> OrderItem { get; set; } = default!;
}