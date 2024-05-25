# README pour l'application resto_tracabilite

## Configuration des Variables d'Environnement

Avant de démarrer l'application, assurez-vous de configurer les variables d'environnement suivantes :
(en créant un fichier .env à la racine du projet)

### Base de Données

DB_SERVER : L'adresse du serveur de base de données.
DB_USER : Le nom d'utilisateur pour accéder à la base de données.
DB_PASSWORD : Le mot de passe associé à l'utilisateur de la base de données.
DB_DATABASE_NAME : Le nom de la base de données utilisée par l'application.

### Jetons d'Authentification JWT

JWT_VALID_ISSUER : L'émetteur valide des jetons JWT.
JWT_VALID_AUDIENCE : L'audience valide des jetons JWT.
JWT_KEY : La clé secrète utilisée pour signer les jetons JWT.

### Configuration de Messagerie Électronique

EMAIL_FROM : L'adresse e-mail à partir de laquelle les e-mails seront envoyés.
EMAIL_SMTP_SERVER : L'adresse du serveur SMTP pour l'envoi d'e-mails.
EMAIL_PORT : Le port utilisé par le serveur SMTP.
EMAIL_USERNAME : Le nom d'utilisateur pour l'authentification SMTP.
EMAIL_PASSWORD : Le mot de passe associé au nom d'utilisateur pour l'authentification SMTP.

## Installation et Utilisation

Pour installer et utiliser l'application, suivez ces étapes :

1. Clonez ce dépôt sur votre machine locale.
2. Configurez les variables d'environnement comme décrit ci-dessus en créant un fichier `.env` à la racine du projet.
3. Appliquer les migrations
4. Démarrez l'application en exécutant `dotnet run`.
5. L'application sera accessible à l'adresse spécifiée dans la documentation.

Note : Les dépendances nécessaires à l'application sont déjà incluses dans le projet via NuGet et ne nécessitent pas d'installation supplémentaire.