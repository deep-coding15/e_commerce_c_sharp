**Pages folder :**
    contains Razor pages and supporting files.
**Each Razor page is a pair of files :**
    - A .cshtml file that has HTML markup with C# code using Razor syntax.
    - A .cshtml.cs file that has c# code that handles page events.

**wwwroot folder :** 
    contains static assets, like HTML files JavaScript files and CSS files.

**appsettings.json :**
    contains configuration data, like connection strings.

**Entity Framework Core (EF Core) :**
    Is an objet/relational mapper (O/RM) that simplifies data access.
    You write the model classes first, and EF Core creates the database.

**Model classes are known as POCO classes (Plain-Old CLR Objects) :**
    Because they don't have a dependency on EF Core. 
    They define the properties of the data that are stored in the database.

**Comment le projet a été généré :**
    1.Prereequisites :
        - Visual Studio Code
        - C# Dev Kit for Visual Studio Code
        - .NET 9 SDK
    2.Creer le projet (Razor Pages Web app) :
        - dotnet new webapp -o E-commerce_c_charp
        - code -r E-commerce_c_charp
    3.Run the app :
        - dotnet dev-certs https --trust : 
            trust the HTTPS development certificate 
    4.Add NuGet packages and EF tools :
        - dotnet tool uninstall --global dotnet-aspnet-codegenerator
        - dotnet tool install --global dotnet-aspnet-codegenerator
        - dotnet tool uninstall --global dotnet-ef
        - dotnet tool install --global dotnet-ef
        - dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
        - dotnet add package Microsoft.EntityFrameworkCore.SQLite --version 9.0.0
        - dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 9.0.0  --Needed for scaffolding
        - dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0           --Needed for scaffolding
        - dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
        - dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
        - dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 9.0.0
        - dotnet add package Microsoft.AspNetCore.Identity.UI --version 9.0.0         
        
        # Installe le fournisseur de base de données temporaire (RAM). 
        # Idéal pour tester l'API sans configurer de SQL Server ou PostgreSQL.
        - dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.0

        # Ajoute des outils de diagnostic pour le débogage. 
        # Affiche des pages d'erreurs détaillées si une migration de base de données échoue au lancement.
        - dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 9.0.0
        
        # Génère automatiquement la documentation OpenAPI et l'interface de test Swagger UI.
        # C'est l'alternative robuste à Swashbuckle pour documenter et tester vos APIs en .NET 9/10.
        - dotnet add package NSwag.AspNetCore --version 14.6.3
        - dotnet add package AutoMapper --version 16.0.0

        - dotnet restore -- faire la mise à jour des réquêtes.

# scaffold the models :
    - dotnet aspnet-codegenerator razorpage --model Product --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Product --referenceScriptLibraries --databaseProvider sqlserver
     
    - dotnet aspnet-codegenerator razorpage --model Order --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Order --referenceScriptLibraries --databaseProvider sqlserver
 
    - dotnet aspnet-codegenerator razorpage --model Category --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Category --referenceScriptLibraries --databaseProvider sqlserver
     
    - dotnet aspnet-codegenerator razorpage --model OrderItem --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/OrderItem --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model User --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/User --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model Cart --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Cart --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model CartItem --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/CartItem --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --viewModel Checkout --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Checkout --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model Product --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Admin/Product --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model Order --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Admin/Order --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model User --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Admin/User --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model OrderItem --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Admin/OrderItem --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator razorpage --model OrderItem --dataContext E_commerce_c_charp.Data.E_commerce_c_charpContext --useDefaultLayout --relativeFolderPath Pages/Admin/OrderItem --referenceScriptLibraries --databaseProvider sqlserver
    - dotnet aspnet-codegenerator identity --files "Account.Login;Account.Register" -dc E_commerce_c_charp.Data.E_commerce_c_charpContext

# Create the initial database schema using EF's migration feature :
EF Core est responsable de la création et de la mise à jour de la structure (le schéma) de la base de données (SQL Server, SQLite, etc.) pour qu'elle corresponde à ces classes.
    The migrations feature in EF Core provides a way to : 
    - Create the initial database schema
    - Incrementally update the database schema to keep it in sync with the app's data model. Existing data in the database is preserved.
    **dotnet ef migrations add InitialCreate :**
        - elle analyse les classes C#
        - Compare avec la dernière migration
        - Génère un fichier de migration qui contient le SQL nécessaire pour créer les tables.
    **dotnet ef database update :** 
        * permet d'appliquer les changements de schéma de base de données en attente, basés sur les migrations C# que vous avez générées précédemment, à votre base de données cible.
        - Synchronisation du modèle et de la base de données
        - Elle effectue les operations suivantes : 
            * Parcourt toutes les classes de migrations générés avec dotnet ef migrations add [nomMigration] qui n'ont pas encore été appliquées à la base de données.
            * Connexion à la Base de Données
            * Exécute les scripts SQL : Pour chaque migration non appliquée, EF Core ecévute le code SQL correspondant.
            * Mise à jour du Suivi : mets a jour une table spéciale __EFMigrationsHistory qui suit quelles sont les migrations déjà appliquées et guarantissent qu'une migration n'est jamais exécutée deux fois.

# Scaffolded Razor Pages in ASP.NET Core:
Go deep into file generated by Scaffolded:
    For Create, Delete, Details and Edit pages
        *In the .cshtml.cs file*
            - The constructor uses *dependency injection* to add the **E_commerce_c_charpContext** to the page
            - OnGetAsync() : For GET requests it returns a list of movies to the Razor Page.
            - When *OnGet* returns *void* or *OnGetAsync* returns *Task*, no return statement is used.
            - When the return type is *IActionResult* or *Task<IActionResult>*, a return statement must be provided. exple: return Page();
        *In the .cshtml file* 
            Razor can transition from HTML into C# or into Razor-specific markup. When an @ symbol is follow by Razor reserved keyword, it transitions into Razor-specific markup, ortherwise it transitions into C#.
            - @page Razor directive : It makes the file an MVC action, which means that it can handle requests. @page must be the first Razor directive on a page. 
            - @model directive : It specifies the type of the model passed to the Razor Page. The model is used in the *@Html.DisplayNameFor()* and *@Html.DisplayFor()* HTML Helpers on the page. *Ex: @Html.DisplayNameFor(model => model.Movie[0].Title)*
            - *ViewData and layout* : 
              - ViewData In the Pages/Shared/_Layout.cshtml file @{ ViewData["Title"] = "Index"; }
              - Layout in the Pages/_ViewStart.cshtml file @{ Layout = "_Layout"; } The preceding markup sets the layout file to Pages/Shared/_Layout.cshtml for all Razor files under the Pages folder. 
        *In the Layout Page* 
            Each page shows the same menu layout where is implemented in the *Pages/Shared/_Layout.cshtml file*. Layout templates allow the HTML container layout to be :
                - Specified in one place.
                - Applied in multiple pages in the site.
                - The @RenderBody() is a placeholder where all the page-specific views show up, wrapped in the layout page.
        *The Create page model*
            - The Validation Tag Helpers (<div asp-validation-summary ...>and <span asp-validation-for ...>) display validation errors. 
            - The Label Tag Helper (<label asp-for="Movie.Title" class="control-label"></label>) generates the label caption and [for] attribute for the Title property.
            - The Input Tag Helper (<input asp-for="Movie.Title" class="form-control">) uses the DataAnnotations attributes and produces HTML attributes needed for jQuery Validation on the client-side.
# Work with a Database (*Seeding in the database*)
To work with datatbase, we can begin by *Seeding in a database*: It means that items will be insert automatically in the Database when the app starts.
Nous avons besoin du *seeding* dans une BD : 
- si la BD se reconstruit trop souvent. 
- on veut le remplir automatiquement avec les données de test.
- Pour des tutoriels.
Nous n'avons pas besoin du *seeding* si :
- on va inserer des données manuellement. 
- on travaille en production.
- on a pas besoin de données par défaut. 

# Update the pages

# Ajout de la recherche
La liaison de modèle [BindProperty]Associe les valeurs de formulaire et les chaînes de requête portant le même nom que la propriété. La liaison de modèle n'est pas sensible à la casse.
[BindProperty(SupportsGet = true)]Est requis pour la liaison sur les requêtes *HTTP GET*. Pour des raisons de sécurité, vous devez autoriser la liaison *GET* des données de requête aux propriétés du modèle de page. Vérifiez les entrées utilisateur avant de les associer aux propriétés. 

# Add a new field to a Razor Page (Database)
Entity Framework Core (EF Core) is used to define the database schema based on the app's model class:
- Add a new field to the model.
- Migrate the new field schema change to the database.
Using EF Code to automatically create and track a database:
- Adds an *__EFMigrationsHistory table* to the database to track whether the schema of the database is in sync with the model classes it was generated from.
- Throws an exception if the model classes aren't in sync with the database.
- Automatic verification that the schema and model are in sync makes it easier to find inconsistent database code issues.
*SqlException: Invalid column name 'Rating'* It is throws because the updated product model class being different than the schema of the Product table of the database : There's no *Rating* column in the database table.
There are few approaches to resolving the error :
1. Let's the Entity Framework automatically drop and re-create the database using the new model class schema. Existing data in the databse is lost. Drop the databse on schema changes and using an initializer to automatically seed the database with test data.
2. Explicitly modify the schema of the existing database so that it matches the model classes.
3. Use Entity Framework Core Migrations to update the database schema.
In the *SeedData.cs* file, after adding the new Column and its value, go in the terminal and write :
- *dotnet build* : for compiling the projet without launch.
- *dotnet ef migrations add nomMigrations* : this command tells the framework to compare the *Product model* with the *Product database schema* and create code to migrate the database schema to the new model.
- *dotnet ef database update* : this command tells the framework to apply the schema changes to the database and to preserve existing data.

# Add Validation rule :
The validation rules are enforced any time a user creates or edits a movie.
- [Required] and [MinimumLength] : property must have a value. The user can enter white space.
- [RegularExpression] : used to limit what characters can be input.
- [Range] : used to constraint a value to within a specified range.
- [StringLength] : used to set a maximum length of a string property, and optionnally its minimum length.
- [DisplayFormat] : decris la façon dont le données sont affichés à l'écran.
- [DataType] : decris la sémantique des données.
Les annotations de données appliquées à la classe modifient le schéma. Ils ne provoquent pas d'exception de la part d'EF cependant, il est recommandé de créer une migration afin que le schéma soit cohérent avec le modèle.

# Add Identity
ASP.NET Core est une API qui prend en charge la fonctionnalité de connexion à l'interface utilisateur (UI).
- Gère les utilisateurs 
- les mots de passe
- les données de profil
- les rôles
- les revendications
- les jetons
- la confirmation par e-mail, et plus encore.
Les utilisateurs peuvent créer un compte avec les informations de connexion enregistrées dans Identity ou utiliser un fournisseur d'authentification externe( Facebook, Google, Microsoft Account et Twitter ). 
On va utiliser Identity pour inscrire, connecter et déconnecter un utilisateur.
ASP.NET Core Identity* ajoute une fonctionnalité d'authentification via l'interface utilisateur aux applications web ASP.NET Core.
Pas besoin de refaire le user avec les attributs (Email, Id, Passwordhash) car Identity les gère automatiquement, on ajoute tout siplement les données métier.
Suppression de *public DbSet<User> User { get; set; }*  dans le fichier *DbContext* Car Identity le crée(les utilisateurs) automatiquement dans la table  [AspNetUsers].
- Créer utilisateur, modifier mot de passe [UserManager]
- lister utilisateurs [UserManager.USers]
- Authentification [Identity]
- Tables metier [DbContext]
# Scaffolded the identity
    - dotnet aspnet-codegenerator identity -dc E_Commerce_c_charp.Data.E_Commerce_c_charpContext --force
    - dotnet aspnet-codegenerator identity --useDefaultUI
    - dotnet aspnet-codegenerator identity -dc E_Commerce_c_charp.Data.E_Commerce_c_charpContext --files "Account.Register;Account.Login;Account.Logout;Account.ResetPassword;Account.ForgotPassword"
    - dotnet aspnet-codegenerator identity -dc E_Commerce_c_charp.Data.E_Commerce_c_charpContext --files "Account.ForgotPassword"
    - dotnet aspnet-codegenerator identity -dc E_Commerce_c_charp.Data.E_Commerce_c_charpContext --files "Account.ForgotPassword;Account.ForgotPasswordConfirmation;Account.ResetPassword;Account.ResetPasswordConfirmation" --force
Ne jamais manipuler les users via DbContext directement.

To change the IDENTITY property of a column, the column needs to be dropped and recreated

# Mettre le mot de passe google dans les user secrets
Dans powershell : [guid]::NewGuid()
dotnet user-secrets set "Email:Smtp:Password" "votre_mot_de_passe_application"
dotnet user-secrets list

# Serilog & ASP.NET Core
# Niveaux de Log dans ASP.NET Core et Serilog
Les niveaux de log sont utilisés pour classer les messages selon leur importance. Voici les principaux niveaux, du plus bas (le plus détaillé) au plus haut (le plus critique) :
- **Trace**  
  Pour les informations très détaillées, généralement utilisées uniquement pour le débogage avancé.
- **Debug**  
  Pour les informations de débogage, utiles pendant le développement.
- **Information**  
  Pour les messages généraux sur le fonctionnement normal de l’application.
- **Warning**  
  Pour les avertissements sur des situations potentiellement problématiques, mais qui ne bloquent pas l’application.
- **Error**  
  Pour les erreurs qui ont empêché une opération de se terminer correctement.
- **Critical**  
  Pour les erreurs critiques qui ont causé l’arrêt ou un dysfonctionnement majeur de l’application.
- **None**  
  Pour désactiver complètement la journalisation.

> Chaque niveau inclut tous les niveaux supérieurs. Par exemple, si tu définis le niveau minimum à "Warning", tu verras aussi les "Error" et "Critical", mais pas les "Information", "Debug" ou "Trace".


# NETTOYER LE PROJET 
dotnet clean
dotnet restore
dotnet build
dotnet watch run

# References : 
1. Tutoriel guidé partie 4 : https://learn.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/sql?view=aspnetcore-9.0&tabs=visual-studio
2. Razor Pages architecture and concepts in ASP.NET Core : https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-9.0&tabs=visual-studio#layout
3. Dependency injection in ASP.NET Core : https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-9.0
4. Razor Pages with Entity Framework Core in ASP.NET Core - Tutorial 1 of 8 : https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-9.0&tabs=visual-studio#asynchronous-code
5. Razor syntax reference for ASP.NET Core : https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-9.0#razor-reserved-keywords
6. Expréssions Lambda : https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
7. Réquêtes LINQ : 
8. Entity Framework Core : https://learn.microsoft.com/en-us/ef/core 

# Commandes sur visual studio code
- dotnet clean : nettoyer le build
- dotnet restore   : Restaurer les packages
- dotnet build     : Compiler le projet sans le lancer
- dotnet run       : Exécuter les Razor Pages
- dotnet watch run : Hot reload => Il recompile à chaque sauvegarde
- dotnet ef migrations script : Générer le SQL complet de la base
- dotnet ef mifrations script -o database.sql : Spécifie le nom du fichier où le code SQL sera généré.
- dotnet ef migrations add nomMigrations : 
- dotnet ef migrations list : 
- dotnet ef migrations remove : supprime le dernier fichier de migration
- dotnet ef database update : applique la nouvelle migration à la base de données
- dotnet ef database update 0(cas où c'est la première migration appliquée) <=> dotnet ef database update nomMigration : Cela supprimera les changements de structure de la base de données de la migration suivant cette migration et retirera l'entrée de la migration dans la table __EFMigrationsHistory. Il execute le Down de la migration suivante.

