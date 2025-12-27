# Récapitulatif du travail sur Razor Pages – Projet E‑commerce

## 1. Dashboard administrateur (Page Razor `/Admin/Dashboard/Index`)

### 1.1. PageModel `IndexModel`

- Utilise `E_commerce_c_charpContext` (EF Core) pour interroger la base.
- Expose une propriété :
  - public DashboardViewModel Dashboard { get; set; } = new();
- Méthode `OnGetAsync()` :
- Remplit les KPIs.
- Calcule la tendance des ventes par mois.
- Calcule la répartition des statuts de commandes.
- Récupère les produits populaires.
- Récupère les commandes récentes.

#### 1.1.1. KPIs de base

- `TotalOrders` : `CountAsync()` sur `Order`.
- `PendingOrders` : `CountAsync()` où `Status == Pending`.
- `Revenue` : `SumAsync()` de `PrixTTC` pour les commandes livrées.
- `AvgOrder` : `Revenue / TotalOrders` (avec vérification de division par zéro).
- `ProductsCount` : `CountAsync()` sur `Product`.
- `UsersCount` : `CountAsync()` sur `Users`.

#### 1.1.2. Tendance des ventes (6 derniers mois)

- Détermine `start` : premier jour du mois courant − 5 mois.
- Filtre les commandes livrées `CreatedAt >= start`.
- Groupement :
    - .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
  .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(x => x.PrixTTC) })
  .OrderBy(x => x.Year).ThenBy(x => x.Month)
    - Remplit :
        - `SalesMonths` : `["Jan", "Feb", ...]`.
        - `SalesTrend` : montants par mois[web:239].


#### 1.1.3. Statuts des commandes (camembert)

- Groupement :

```

var statusGroups = await _context.Order
.GroupBy(o => o.Status)
.Select(g => new { Status = g.Key, Count = g.Count() })
.ToListAsync();

Dashboard.OrderStatusCounts = statusGroups
.ToDictionary(x => x.Status.ToString(), x => x.Count);

```

#### 1.1.4. Produits populaires & commandes récentes

- Produits populaires (TOP 5) :

```

var topProducts = await _context.OrderItem
.Include(oi => oi.Product)
.GroupBy(oi => oi.Product)
.Select(g => new { Product = g.Key, SoldCount = g.Sum(x => x.Quantity) })
.OrderByDescending(x => x.SoldCount)
.Take(5)
.ToListAsync();

```

- Conversion vers `TopProductViewModel` (Rank, Name, SoldCount, Price).
- Commandes récentes (TOP 5) : tri par `CreatedAt` décroissant, projection vers `RecentOrderViewModel` (Number, Date, Total, StatusLabel).

---

## 2. ViewModel `DashboardViewModel`

### 2.1. Propriétés principales

```

public class DashboardViewModel
{
// KPIs
public int TotalOrders { get; set; }
public int PendingOrders { get; set; }
public decimal Revenue { get; set; }
public decimal AvgOrder { get; set; }
public int ProductsCount { get; set; }
public int UsersCount { get; set; }

    // Ventes par mois
    public List<string> SalesMonths { get; set; } = new();
    public List<decimal> SalesTrend { get; set; } = new();
    
    // Statuts de commandes
    public Dictionary<string, int> OrderStatusCounts { get; set; } = new();
    
    // Produits populaires
    public List<TopProductViewModel> TopProducts { get; set; } = new();
    
    // Commandes récentes
    public List<RecentOrderViewModel> RecentOrders { get; set; } = new();
    }

public class TopProductViewModel
{
public int Rank { get; set; }
public string Name { get; set; } = "";
public int SoldCount { get; set; }
public decimal Price { get; set; }
}

public class RecentOrderViewModel
{
public string Number { get; set; } = "";
public DateTime Date { get; set; }
public decimal Total { get; set; }
public string StatusLabel { get; set; } = "";
}

```

---

## 3. Page Razor `Index.cshtml` – Dashboard

### 3.1. Liaison modèle & sérialisation

```

@page
@model E_commerce_c_charp.Pages_Admin_Dashboard.IndexModel
@{
ViewData["Title"] = "Dashboard";

    var vm = Model.Dashboard;
    
    var salesMonthsJson = System.Text.Json.JsonSerializer.Serialize(vm.SalesMonths);
    var salesTrendJson  = System.Text.Json.JsonSerializer.Serialize(vm.SalesTrend);
    var orderStatusJson = System.Text.Json.JsonSerializer.Serialize(vm.OrderStatusCounts);
    }

```

### 3.2. Contenu principal

- **Cartes KPI** : 4 colonnes Bootstrap avec :
  - Commandes totales + en attente.
  - Chiffre d’affaires + panier moyen.
  - Nombre de produits.
  - Nombre d’utilisateurs.
- **2 graphiques** :
  - Line chart “Tendance des ventes” (mois / ventes).
  - Pie chart “Statut des commandes”.
- **2 listes** :
  - Produits populaires (Top 5).
  - Commandes récentes (Top 5)[web:248][web:250][web:266].

### 3.3. Passage C# → JavaScript

```

@section Scripts {
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
        const salesMonths     = @Html.Raw(salesMonthsJson);
        const salesTrend      = @Html.Raw(salesTrendJson);
        const orderStatusDict = @Html.Raw(orderStatusJson);
    </script>
}

```

### 3.4. Graphique ligne (tendance des ventes)

```

const salesCtx = document.getElementById('salesTrendChart');

new Chart(salesCtx, {
type: 'line',
data: {
labels: salesMonths,
datasets: [{
label: 'ventes',
data: salesTrend,
borderColor: 'rgba(54, 162, 235, 1)',
backgroundColor: 'rgba(54, 162, 235, 0.1)',
pointRadius: 4,
tension: 0.3,
fill: true
}]
},
options: {
responsive: true,
plugins: { legend: { display: true } },
scales: { y: { beginAtZero: true } }
}
});

```

### 3.5. Graphique camembert (statut des commandes)

Couleurs sémantiques recommandées pour l’UX[web:272][web:278] :

- Livrée : `#22C55E` (vert – succès).
- Confirmée : `#3B82F6` (bleu – positif).
- En préparation : `#FACC15` (jaune/orange – en cours).
- Expédiée : `#8B5CF6` (violet – état intermédiaire).
- Annulée : `#EF4444` (rouge – échec).

```

const statusLabels = Object.keys(orderStatusDict);
const statusValues = Object.values(orderStatusDict);

const statusCtx = document.getElementById('orderStatusChart');

new Chart(statusCtx, {
type: 'pie',
data: {
labels: statusLabels,
datasets: [{
data: statusValues,
backgroundColor: [
'\#22C55E', // Livrée
'\#3B82F6', // Confirmée
'\#FACC15', // En préparation
'\#8B5CF6', // Expédiée
'\#EF4444'  // Annulée
]
}]
},
options: {
responsive: true,
plugins: { legend: { position: 'top' } }
}
});

```

---

## 4. Page Razor “Analytics” (Analytique & Rapports)

Une deuxième page Razor “Analytics” complète le Dashboard.

### 4.1. Indicateurs calculés

- **Panier moyen**  
  - Moyenne des montants `PrixTTC` des commandes.
  - Affiché dans une carte avec format monétaire (Average Order Value)[web:320][web:322].

- **Ventes par catégorie**  
  - Groupement des `OrderItem` par `Product.Category`.  
  - Somme des quantités ou montants par catégorie.  
  - Stocké dans un `Dictionary<string, int/decimal>` et envoyé à Chart.js (bar chart) pour afficher les ventes par catégorie[web:248][web:312].

- **Taux de conversion**  
  - `Taux = CommandesLivrées / TotalCommandes * 100`.
  - Affiché en pourcentage dans une carte (“Commandes livrées / Total”)[web:320][web:325][web:327].

- **Analyse du stock**  
  - Comptage des produits par état :
    - **Stock suffisant** : quantité > seuil (ex. 20).
    - **Stock faible** : 1 ≤ quantité ≤ seuil.
    - **Rupture de stock** : quantité = 0.  
  - Présentation en liste avec icônes/couleurs :
    - Vert pour stock suffisant.
    - Orange pour stock faible.
    - Rouge pour rupture[web:325][web:327].

### 4.2. Intégration avec Chart.js

- Même approche que pour le Dashboard :
  - Sérialisation des dictionnaires (`VentesParCategory`, etc.) en JSON.
  - Utilisation de `Object.keys()` / `Object.values()` côté JS pour remplir `labels` et `data`.
  - Bar chart “Ventes par catégorie” avec personnalisation de l’épaisseur des barres (`barThickness`) et des espaces (`barPercentage`, `categoryPercentage`) pour un rendu plus lisible[web:159][web:179][web:194].

---

## 5. Points techniques appris / utilisés

- Conception de **ViewModels spécialisés** pour les dashboards Razor.
- Utilisation d’Entity Framework Core pour produire des **statistiques agrégées** (`GroupBy`, `Sum`, `Count`, `Average`)[web:227][web:239].
- Passage de données serveur C# vers JavaScript avec `System.Text.Json.JsonSerializer` et `@Html.Raw`.
- Intégration de **Chart.js** dans Razor Pages :
  - Graphiques `line`, `bar`, `pie`.
  - Configuration de la largeur des barres (`barThickness`) et de l’espace (`barPercentage`, `categoryPercentage`).
- Choix de **couleurs sémantiques** pour les statuts de commandes (succès, avertissement, erreur, info) pour une meilleure UX[web:272][web:278].
- Utilisation de Bootstrap pour organiser les KPIs, graphiques et listes dans un **dashboard administrateur** moderne[web:256][web:262].

Ce document résume l’architecture et les principales fonctionnalités mises en place pour les pages Razor de **Dashboard** et **Analytics** de l'application e‑commerce.
