using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Models;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models.Requests;

namespace E_commerce_c_charp.EndPoints;
public static class AdminProductEndpoints
{
    public static void MapAdminProductEndpoints(this WebApplication app)
    {
        var AdminProductItems = app.MapGroup("/Admin/Product");
        AdminProductItems.MapGet("", () => {
            return Results.Redirect($"/Admin/Product/Index");
        });
    }
}