# Journal – 24/12/2025

## Modèles et données

- Finalisation du modèle `Order` : identifiant, lien utilisateur, totaux, infos client, statut, lignes de commande.
- Amélioration du modèle `Product` : ajout d’un SKU, nettoyage des attributs de validation, relation avec `OrderItem`.
- Clarification des statuts de commande :
  - `Pending` : commande créée, paiement ou traitement en attente.
  - `Completed` : paiement validé, commande préparée/expédiée.
  - `Delivered` : livraison confirmée.
  - `Cancelled` : commande annulée (par client ou vendeur).
- Les modèles sont conçus pour être évolutifs et réutilisables dans d’autres parties du site (admin, API, emails).

## ViewModels

- Création/ajustement des ViewModels :
  - `OrderItemViewModel` : nom produit, variante, quantité, prix unitaire, image.
  - `OrderSummaryViewModel` : numéro commande, date, statut, adresse, total, liste des articles.
  - `OrderDashboardViewModel` : statistiques globales (total dépensé, commandes en cours, livrées, articles achetés, nombre de commandes).
- Les ViewModels permettent de séparer la logique métier de l’affichage, rendant la page plus lisible et modifiable.

## Razor Pages – Orders

- Création de la page Razor `Pages/Order/Index.cshtml` et de son PageModel `Index.cshtml.cs`.
- Chargement des commandes de l’utilisateur connecté via EF Core (`Order` + `OrderItem` + `Product`).
- Calcul des indicateurs : total dépensé, nombre de commandes, commandes livrées, articles achetés.
- Affichage d’une UI moderne :
  - Cartes de commandes avec résumé (ID, date, statut, total).
  - Liste des produits commandés (image, nom, quantité, prix).
  - Adresse de livraison et date de livraison estimée.
  - Actions (détails, renouveler, annuler).
  - Timeline des statuts (colorée selon l’état).
- La structure Razor Pages facilite la séparation des préoccupations et la réutilisation des composants.

## Divers
- Explication du concept de **SKU** : code interne unique pour identifier un produit (stock, back-office, historique).
- Proposition d’exemples concrets pour plusieurs produits (Logitech, Sony, MacBook, Dell, Samsung, iPhone) exemple : *PHO-APP-IP15PRO*, *LAP-DEL-XPS15*, *HEAD-SON-WH1000XM5* .
- Architecture pensée pour être évolutif : ajout facile de nouveaux états ou champs sans impacter l’ensemble du projet.
- Possibilité d’étendre la structure pour d’autres pages (ex. `Order/Details` avec `CheckoutViewModel`).
