# Résumé des travaux sur le projet e‑commerce

## 1. API panier (Minimal APIs)

- Création d’un groupe d’endpoints `/api/Cart` via une classe statique `CartEndpointsApi` avec une méthode d’extension `MapCartEndpointsApi(WebApplication app)`.
- Mise en place des routes suivantes :
  - `POST /api/Cart/add` : ajouter un produit au panier.
  - `POST /api/Cart/suppr` : diminuer la quantité d’un produit.
  - `POST /api/Cart/handleProduct` : fixer la quantité exacte d’un produit (0, 1, 2, …).
  - `POST /api/Cart/remove` : supprimer complètement un produit du panier.
  - `GET /api/Cart` : rediriger vers le panier du client `/Cart/Index?UserId=...`.
  - `GET /api/Cart/Details/{id:int}` : rediriger vers `/Cart/Details?id=...`.
- Chaque endpoint :
  - Valide le DTO de requête (`AddToCartRequest`, `RemoveToCartRequest`) reçu en JSON.
  - Récupère l’utilisateur connecté via `UserManager<User>` et `HttpContext`.
  - Charge le panier avec `Include(c => c.Items).ThenInclude(i => i.Product)` pour avoir les produits liés.
  - Met à jour les lignes du panier (`CartItem`) selon l’action (ajout, diminution, mise à jour, suppression).
  - Sauvegarde en base avec `SaveChangesAsync()`.
  - Recalcule les totaux du panier :
    - `PrixHT` = somme des `Quantity * Product.Price`.
    - `PrixTVA` = `PrixHT * Order.TVA`.
    - `PrixTTC` = `PrixHT + PrixTVA`.
  - Retourne un JSON standard : `{ success, PrixHT, PrixTVA, PrixTTC }`.

## 2. Calculs C# (totaux du panier) et corrections de null

- Mise en place des calculs côté serveur :
  - `NbArticles` : nombre de produits distincts dans le panier.
  - `PrixHT`, `PrixTVA`, `PrixTTC` calculés à partir des items du panier.
- Correction d’un `NullReferenceException` dans le `Sum` en sécurisant :
  - Chargement des produits avec `ThenInclude(i => i.Product)`.
  - Gestion des cas où `cart`, `cart.Items` ou `ci.Product` peuvent être `null`.
  - Exemple de sum robuste :
    - `cart.Items.Sum(ci => (ci?.Quantity ?? 0) * (ci?.Product?.Price ?? 0m))`.

## 3. JavaScript côté client (mise à jour temps réel)

- Gestion d’un input de quantité avec l’événement `change` :
  - Passage de `e.data` (incorrect) à `e.target.value` pour récupérer la valeur saisie.
  - Conversion en nombre (`parseInt` ou `Number`) et contrôle `<= 0`.
- Appels `fetch` vers l’API `/api/Cart/...` :
  - Envoi de JSON avec `ProductId` et `Quantity`.
  - Récupération de la réponse JSON (totaux) et mise à jour du DOM :
    - Zones `prix-ht`, `prix-tva`, `prix-ttc` mises à jour en fonction de `data.prixHT`, `data.prixTVA`, `data.prixTTC`.
  - Gestion des cas `NaN` côté JS en affichant `0` si nécessaire.

## 4. Page Razor Checkout (commande)

- Création d’un `CheckoutViewModel` :

  ```
  public class CheckoutViewModel
  {
      public List<CartItemViewModel> Items { get; set; } = new();
      public decimal PrixHT { get; set; }
      public decimal PrixTVA { get; set; }
      public decimal PrixTTC { get; set; }

      public string FullName { get; set; } = "";
      public string Email { get; set; } = "";
      public string Address { get; set; } = "";
      public string City { get; set; } = "";
      public string Phone { get; set; } = "";
  }
  ```

- Mise en place de `CartItemViewModel` avec une propriété calculée `TotalPrice` :
  - `TotalPrice => Quantity * (Product?.Price ?? 0)`.
- PageModel `Pages/Checkout/Index.cshtml.cs` :
  - Injection de `E_commerce_c_charpContext` et `UserManager<User>`.
  - `[BindProperty] public CheckoutViewModel Order { get; set; }`.
  - `OnGetAsync()` :
    - Récupération de l’utilisateur connecté.
    - Chargement du panier correspondant à cet utilisateur avec les `CartItem` et `Product`.
    - Projection du panier vers `Order.Items` (liste de `CartItemViewModel`).
    - Calcul des totaux via `ComputeTotals()` (PrixHT, TVA, TTC).
  - `OnPostAsync()` :
    - Validation du modèle.
    - Récupération de l’utilisateur.
    - Recalcul des totaux.
    - Création d’une entité `Order` avec les informations client et les totaux.
    - Création des `OrderItem` à partir des `Order.Items`.
    - Sauvegarde de la commande en base.
    - Redirection vers une page de reçu `/Orders/Receipt?id=...`.

## 5. Entité Order, statut et reçu

- Définition / extension de la classe `Order` :
  - Propriétés : `Id`, `UserId`, `User`, `TotalAmount`, `PrixHT`, `PrixTVA`, `PrixTTC`, `FullName`, `Email`, `Address`, `City`, `Phone`, `Status`, `CreatedAt`.
  - `Status` : enum avec `Pending`, `Cancelled`, `Completed`, `Delivered`.
  - `TVA` : constante `0.2M`.
  - Navigation `List<OrderItem> Items` pour les lignes de commande.
- Mise en place du calcul des totaux à partir du ViewModel et copie vers `Order` lors du POST.
- Concept de page de reçu (`Pages/Orders/Receipt`) :
  - Chargement de l’`Order` par son `Id` depuis la BDD.
  - Include des `Items` et du `Product`.
  - Affichage des informations client, des lignes de commande et des totaux comme reçu.

## 6. Navigation et intégration côté UI

- Bouton “Passer la commande” sur la page Cart :
  - Fonction JS qui redirige vers `/Checkout` avec `window.location.href`.
- Page Checkout :
  - Affiche la liste des produits du panier (nom, quantité, prix unitaire, total ligne).
  - Affiche les totaux (HT, TVA, TTC).
  - Formulaire pour les informations client (nom, email, adresse, ville, téléphone) lié à `CheckoutViewModel`.
- Utilisation de `@section Scripts { ... }` pour ajouter les scripts spécifiques à certaines pages tout en respectant le layout Razor.

## 7. Page Razor Receipt (affichage du reçu)
Création de la page Razor Pages/Order/Receipt.cshtml avec la directive @page "{id:int}" et le modèle *ReceiptModel* pour afficher le reçu d’une commande par son Id.
Affichage des informations de la commande : numéro de commande (Order.Id), date (Order.CreatedAt.ToLocalTime()), informations client (Order.User.Nom, UserName, Email, PhoneNumber) et message de remerciement.
Affichage détaillé des lignes de commande dans un tableau HTML : nom du produit (item.Product?.Name), quantité (item.Quantity), prix unitaire (item.UnitPrice), total ligne (item.Quantity * item.Product.Price) et total TTC (Order.TotalAmount).

## 8. PageModel *ReceiptModel* et chargement de la commande
Ajout du PageModel *ReceiptModel* dans Pages/Order/Receipt.cshtml.cs, injectant le E_commerce_c_charpContext pour accéder à la base de données.
Implémentation de OnGetAsync(int id) qui :
Charge l’entité Order correspondante avec Include(o => o.User) et Include(o => o.Items).ThenInclude(i => i.Product) pour récupérer l’utilisateur et les produits liés.
Retourne NotFound() si la commande n’existe pas, sinon renvoie la page avec Order alimenté pour l’affichage.​

## 9. Bouton “Télécharger le reçu” (formulaire et handler)
Ajout dans Receipt.cshtml d’un formulaire dédié au téléchargement du reçu :
method="get" et asp-page-handler="Download" pour cibler un handler spécifique côté PageModel.
Champ caché <input type="hidden" name="id" value="@Model.Order.Id" /> pour renvoyer l’Id de la commande.
Création du handler OnGetDownloadAsync(int id) dans ReceiptModel pour gérer la requête GET avec handler Download :
Récupération de la commande par id avec les mêmes Include que dans OnGetAsync.
Retour NotFound() si aucune commande ne correspond.​

## 10. Génération du reçu en HTML pour téléchargement
Construction du contenu du reçu au format HTML dans OnGetDownloadAsync à l’aide d’un StringBuilder, incluant :
Le titre “Reçu de commande #...”, la date, le bloc “Client” (nom, nom utilisateur, email, téléphone).
Un tableau HTML des lignes de commande (produit, quantité, prix unitaire, total ligne).
Le total TTC et un texte “Merci pour votre commande.”.
Conversion du HTML en tableau d’octets via Encoding.UTF8.GetBytes(sb.ToString()).
Retour d’un fichier à télécharger avec return File(bytes, "text/html", fileName); où fileName suit un format du type recu-commande-{order.Id}.html, permettant au navigateur de proposer le téléchargement d’un fichier HTML de reçu.​

## 11. Cohérence globale avec le reste du flux (Cart → Checkout → Order → Receipt)
Intégration de la nouvelle page Receipt dans le flux déjà en place :
Après le OnPostAsync() de la page Checkout qui crée l’entité Order, la redirection va désormais vers /Order/Receipt?id=... (ou route équivalente avec @page "{id:int}") pour afficher le reçu.
Harmonisation du calcul et de l’affichage :
Les totaux déjà calculés lors du Checkout (PrixHT, PrixTVA, PrixTTC, TotalAmount) sont maintenant affichés de manière cohérente sur la page de reçu et dans le fichier HTML téléchargé.