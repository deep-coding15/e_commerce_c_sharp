# Journal des Avancements – Projet E-commerce C#

## Avancement du 22 decembre

- Ajout des user secrets pour sécuriser la configuration du mot de passe SMTP de Google.
- Configuration de `SmtpEmailSender` avec les identifiants SMTP sécurisés.
- Suppression des pages Identity (login, logout, register).
- Recréation complète des pages Identity via le code generator d’Identity.
- Mise à jour de la structure du projet pour refléter les nouvelles pages et la configuration sécurisée.

## Avancement du 23 decembre

- Intégration de Serilog pour la gestion des logs.
- Configuration de Serilog : écriture des logs dans un fichier (niveaux Warning, Error, Critical) et affichage des messages Information dans la console.
- Création d’un fichier Markdown expliquant les niveaux de log et leur utilisation.
