# ======================================================
# ÉTAPE 1 : IMAGE DE BASE (RUNTIME)
# ======================================================
# Utilise l'image officielle .NET 9 runtime (légère, pour exécuter l'app)
# ======================================================
# ÉTAPE 1 : IMAGE DE BASE (RUNTIME)
# ======================================================
# Utilise l'image officielle .NET 9 runtime (légère, pour exécuter l'app)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# Définit le répertoire de travail dans le conteneur
# Définit le répertoire de travail dans le conteneur
WORKDIR /app
# Expose le port 8080 (Render utilisera ce port)
# Expose le port 8080 (Render utilisera ce port)
EXPOSE 8080
# Configure ASP.NET Core pour écouter sur le port 8080
ENV ASPNETCORE_URLS=http://+:8080
# Active le mode Production (utilise appsettings.Production.json)
ENV ASPNETCORE_ENVIRONMENT=Production
# Configure ASP.NET Core pour écouter sur le port 8080
ENV ASPNETCORE_URLS=http://+:8080
# Active le mode Production (utilise appsettings.Production.json)
ENV ASPNETCORE_ENVIRONMENT=Production

# ======================================================
# ÉTAPE 2 : COMPILATION (BUILD)
# ======================================================
# Utilise l'image SDK (contient les outils de compilation)
# ======================================================
# ÉTAPE 2 : COMPILATION (BUILD)
# ======================================================
# Utilise l'image SDK (contient les outils de compilation)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Répertoire de travail pour la compilation
# Répertoire de travail pour la compilation
WORKDIR /src
# Copie uniquement le fichier .csproj (pour optimiser le cache Docker)
# ⚠️ REMPLACEZ "VotreFichier.csproj" par le nom réel de votre fichier .csproj
# Copie uniquement le fichier .csproj (pour optimiser le cache Docker)
# ⚠️ REMPLACEZ "VotreFichier.csproj" par le nom réel de votre fichier .csproj
COPY ["E-commerce_c_charp.csproj", "./"]
# Restaure les dépendances NuGet (télécharge les packages)
RUN dotnet restore "E-commerce_c_charp.csproj"
# Copie tout le code source du projet
COPY . .

# Compile l'application en mode Release (optimisé)

# Compile l'application en mode Release (optimisé)
RUN dotnet build "E-commerce_c_charp.csproj" -c Release -o /app/build

# ======================================================
# ÉTAPE 3 : PUBLICATION
# ======================================================
# Utilise l'étape build précédente
# ======================================================
# ÉTAPE 3 : PUBLICATION
# ======================================================
# Utilise l'étape build précédente
FROM build AS publish
# Publie l'application (prépare les fichiers pour le déploiement)
# /p:UseAppHost=false = ne crée pas d'exécutable natif
RUN dotnet publish "E-commerce_c_charp.csproj" -c Release -o /app/publish /p:UseAppHost=false
# Publie l'application (prépare les fichiers pour le déploiement)
# /p:UseAppHost=false = ne crée pas d'exécutable natif
RUN dotnet publish "E-commerce_c_charp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ======================================================
# ÉTAPE 4 : IMAGE FINALE (LÉGÈRE)
# ======================================================
# Revient à l'image de base (sans SDK, juste le runtime = plus léger)
# ======================================================
# ÉTAPE 4 : IMAGE FINALE (LÉGÈRE)
# ======================================================
# Revient à l'image de base (sans SDK, juste le runtime = plus léger)
FROM base AS final
WORKDIR /app
# Copie uniquement les fichiers publiés depuis l'étape "publish"
# Copie uniquement les fichiers publiés depuis l'étape "publish"
COPY --from=publish /app/publish .
# Commande qui s'exécute au démarrage du conteneur
# ⚠️ REMPLACEZ "VotreFichier.dll" par le nom réel de votre DLL
# Commande qui s'exécute au démarrage du conteneur
# ⚠️ REMPLACEZ "VotreFichier.dll" par le nom réel de votre DLL
ENTRYPOINT ["dotnet", "E-commerce_c_charp.dll"]