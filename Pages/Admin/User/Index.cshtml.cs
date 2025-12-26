using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using E_commerce_c_charp.ViewModels.Admin;
using NuGet.Protocol;


namespace E_commerce_c_charp.Pages_Admin_User
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;
        private readonly UserManager<User> _userManager;

        private readonly ILogger<User> _logger;

        public IndexModel(
            E_commerce_c_charpContext context,
            UserManager<User> userManager,
            ILogger<User> logger
        )
        {
            _context     = context;
            _userManager = userManager;
            _logger      = logger;
        }

        public IList<UserViewModel> Users  { get;set; } = default!;

        public int TotalUsers     { get; set; }
        public int AdminCount     { get; set; }
        public int ClientCount    { get; set; }
        public int NewUsers30Days { get; set; }

        public async Task OnGetAsync()
        {
            //var query   = await _context.Users.AsQueryable();
            var users     = _context.Users;
            var userRoles = _context.UserRoles;
            var roles     = _context.Roles;

            TotalUsers = users.Count();

            AdminCount =
                (from u in users
                join ur in userRoles on u.Id equals ur.UserId
                join r in roles on ur.RoleId equals r.Id
                where r.Name == "Admin"
                select u.Id).Distinct().Count();

            ClientCount =
                (from u in users
                join ur in userRoles on u.Id equals ur.UserId
                join r in roles on ur.RoleId equals r.Id
                where r.Name == "Client"
                select u.Id).Distinct().Count();

            NewUsers30Days = users.Count(u => u.CreatedAt >= DateTime.Now.AddDays(-30));

            /*Users = query
                    .Select(u => new UserViewModel {
                        Id = u.FindFirstValue("Id").Value,
                        Name = u.FindFirstValue("FullName").Value,
                        Email = u.FindFirstValue("Email").Value,
                        Role = u.FindFirstValue("Role").Value,
                        RegistrationDate = u.FindFirstValue("CreatedAt").Value,
                        IsActive = "u.IsActive",
                        OrdersCount = u.FindFirstValue("Orders").Value.Count
                    }).ToList(); */
            /* Users = query
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Name = u.FullName,        // ou u.UserName
                    Email = u.Email,
                    Role = u.Role,            // si propriété, sinon jointure UserRoles
                    RegistrationDate = u.CreatedAt,
                    IsActive = u.IsActive,
                    OrdersCount = u.Orders.Count
                })
                .ToList(); */
            var hi = from r in roles select r;
            _logger.LogWarning(hi.ToJson());
            var query =
                from u in users
                join ur in userRoles on u.Id equals ur.UserId
                join r in roles on ur.RoleId equals r.Id
                select new UserViewModel
                {
                    Id = u.Id,
                    Name = u.UserName,
                    Email = u.Email ?? "",
                    Role = r.Name,
                    RegistrationDate = u.CreatedAt,
                    IsActive = true,
                    OrdersCount = u.Orders.Count
                };
            _logger.LogWarning(query.ToJson());
            Users = query.ToList();

        }
    }
}
