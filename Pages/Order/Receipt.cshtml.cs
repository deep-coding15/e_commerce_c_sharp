using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using E_commerce_c_charp.Data;
using E_commerce_c_charp.Models;
using System.Text;
using Microsoft.Net.Http.Headers;
namespace E_commerce_c_charp.Pages.Order
{
    public class ReceiptModel : PageModel
    {
        private readonly E_commerce_c_charpContext _context;

        public ReceiptModel(E_commerce_c_charpContext context)
        {
            _context = context;
        }

        public E_commerce_c_charp.Models.Order Order { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Order
                .Include(u => u.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (Order == null)
                return NotFound();

            return Page();
        }

        // Handler pour Télécharger le reçu (HTML)
        public async Task<IActionResult> OnGetDownloadAsync(int id)
        {Console.WriteLine("Download handler");
            var order = await _context.Order
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var sb = new StringBuilder();
            sb.AppendLine("<html><head><meta charset=\"utf-8\" /><title>Reçu</title></head><body>");
            sb.AppendLine($"<h2>Reçu de commande #{order.Id}</h2>");
            sb.AppendLine($"<p>Date : {order.CreatedAt.ToLocalTime()}</p>");

            sb.AppendLine("<h4>Client</h4>");
            sb.AppendLine($"<p><strong>Nom :</strong> {order.User?.Nom}</p>");
            sb.AppendLine($"<p><strong>Nom Utilisateur :</strong> {order.User?.UserName}</p>");
            sb.AppendLine($"<p><strong>Email :</strong> {order.User?.Email}</p>");
            sb.AppendLine($"<p><strong>Téléphone :</strong> {order.User?.PhoneNumber}</p>");

            sb.AppendLine("<hr />");
            sb.AppendLine("<h4>Détails de la commande</h4>");

            sb.AppendLine("<table border=\"1\" cellspacing=\"0\" cellpadding=\"5\">");
            sb.AppendLine("<tr><th>Produit</th><th>Qté</th><th>PU</th><th>Total ligne</th></tr>");

            foreach (var item in order.Items)
            {
                var totalLigne = item.Quantity * item.UnitPrice;
                sb.AppendLine($"<tr><td>{item.Product?.Name}</td><td>{item.Quantity}</td><td>{item.UnitPrice:0.00} €</td><td>{totalLigne:0.00} €</td></tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine($"<p><strong>Total TTC :</strong> {order.TotalAmount:0.00} €</p>");
            sb.AppendLine("<p>Merci pour votre commande.</p>");
            sb.AppendLine("</body></html>");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var fileName = $"recu-commande-{order.Id}.html";

            return File(bytes, "text/html", fileName);
        }

    }
}
