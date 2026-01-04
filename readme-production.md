1. Préparation de l'application
    # Tester en mode production localement
    *dotnet run --environment Production*

    # Créer une build de production : 
    Générer le package de déploiement localement
    *dotnet publish -c Release -o ./publish*
2. Installing necessary extensions
   1. Azure App Service : An Azure App Service management extension for Visual Studio Code.
   2. Azure Resources   : An extension for viewing and managing Azure resources.
3. Create an Azure Account
   1. Allez sur [Azure free](azure.microsoft.com/free) ou [Azure Pricing](https://azure.microsoft.com/fr-fr/pricing/purchase-options/azure-account?icid=azurefreeaccount)
   2. Créez un compte gratuit (200$ de crédit + services gratuits)
   3. Connectez-vous
   Moi je me suis connectée avec mon email institutionnel. 
4. Étape 3 : Se connecter à Azure depuis VS Code
    Dans VS Code, cliquez sur l'icône Azure dans la barre latérale gauche (logo Azure)
    Cliquez sur Sign in to Azure
    Suivez les instructions pour vous connecter
5. Vérification rapide :
    Dans VS Code, regardez dans la barre latérale Azure :
    En haut de la section Azure, vous devriez voir votre nom/email
    Question importante : Voyez-vous votre abonnement "Azure for Students" dans la liste ?
    Si OUI ✅
         de problème ! Continuez normalement avec les étapes que je vous ai données.
    Si NON ❌
        Il faut changer de compte :
        Dans la barre Azure, cliquez sur l'icône compte (en haut de la section Azure)
        Cliquez sur Sign out
        Cliquez sur Sign in to Azure
        Important : Choisissez ou ajoutez votre compte scolaire (celui avec Azure Student)
        Une fois connecté, vous devriez voir l'abonnement "Azure for Students"
6. Étape 4 : Préparer l'application pour le déploiement
   *dotnet publish -c Release -o ./publish*
7. Étape 4 : Créer le Resource Group
    Dans la barre latérale Azure, développez Resources
    Développez votre abonnement Azure for Students
    Vous devriez voir Resource Groups
    Clic droit sur Resource Groups → Create Resource Group...
    Entrez un nom (Nka_aMarket) north europe
    Choisissez la région : (West Europe)
# Configuration de la base de données
## Database Info
- Server Name: SQL6032.site4now.net
- Database Name: db_ac34a8_nkaamarket
- User Name: db_ac34a8_nkaamarket_admin
- Data Source=SQL6032.site4now.net;Initial Catalog=db_ac34a8_nkaamarket;User Id=db_ac34a8_nkaamarket_admin;Password=YOUR_DB_PASSWORD
   Étape 5 : Créer l'App Service

Dans la même barre Azure, cherchez App Services (ou sous Resources)
Clic droit sur App Services → Create New Web App... (Advanced)

[Azure student account](https://portal.azure.com/?Microsoft_Azure_Education_correlationId=73a4bbeb-f192-4afc-98ae-6b8373490b57&Microsoft_Azure_Education_newA4E=true&Microsoft_Azure_Education_asoSubGuid=64ded925-0dd4-467f-bf5b-582549c35e16#view/Microsoft_Azure_Education/EducationMenuBlade/~/overview)