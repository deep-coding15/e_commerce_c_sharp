@echo off
setlocal

:: Chemin vers le projet ASP.NET Core Razor Pages
set projectPath=.

:: Recherche du fichier .csproj
for %%f in (*.csproj) do (
    set csprojFile=%%f
    goto :found
)

echo Aucun fichier .csproj trouvé dans le répertoire actuel.
exit /b 1

:found
echo Fichier .csproj trouvé : %csprojFile%

:: Restaure les dépendances du projet
dotnet restore "%csprojFile%"

:: Compile le projet
dotnet build "%csprojFile%"

:: Exécute l'application
dotnet run --project "%csprojFile%"

endlocal
