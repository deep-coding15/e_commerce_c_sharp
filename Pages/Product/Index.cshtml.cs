using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce_c_charp.Pages_Product
{
    public class IndexModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public IndexModel(E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        /* Contient le texte saisis par l'utilisateur */
        public string? SearchString {get; set;} 
        /* Contient la liste des categories : Permet à un utilisateur de selectionner une catégorie à la liste. */
        public SelectList? Category {get; set;} 

        [BindProperty(SupportsGet = true)]
        /* Contient la catégorie sélectionnée par l'utilisateur */
        public string? ProductCategory{get; set;}

        /* For GET requests it returns a list of movies to the Razor Page.*/
        public async Task OnGetAsync() 
        {
            /* Requête LINQ qui récupère toutes les catégories de produit de la base de données */
            IQueryable<string> categoryQuery = from p in _context.Product 
                                                    orderby p.Category.Name select p.Category.Name;
            // Réquête LINQ pour sélectionner les produits en fonction de la sélection de l'utilisateur
            var products = from p in _context.Product select p; 
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                products = products.Where(ss => ss.Name.Contains(SearchString));
            }
            if (!string.IsNullOrWhiteSpace(ProductCategory))
            {
                products = products.Where(pc => pc.Category.Name == ProductCategory);
            }

            Category = new SelectList(await categoryQuery.Distinct().ToListAsync());
            Product = await products.Include(p => p.Category).ToListAsync();
        }
    }
}
