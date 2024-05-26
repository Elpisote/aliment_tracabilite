# README pour le BACKEND de l'application resto_tracabilite 

## Configuration des Variables d'Environnement

Avant de d�marrer l'application, assurez-vous de configurer les variables d'environnement suivantes :
(en cr�ant un fichier .env � la racine du projet)

### Base de Donn�es

DB_SERVER : L'adresse du serveur de base de donn�es.
DB_USER : Le nom d'utilisateur pour acc�der � la base de donn�es.
DB_PASSWORD : Le mot de passe associ� � l'utilisateur de la base de donn�es.
DB_DATABASE_NAME : Le nom de la base de donn�es utilis�e par l'application.

### Jetons d'Authentification JWT

JWT_VALID_ISSUER : L'�metteur valide des jetons JWT.
JWT_VALID_AUDIENCE : L'audience valide des jetons JWT.
JWT_KEY : La cl� secr�te utilis�e pour signer les jetons JWT.

### Configuration de Messagerie �lectronique

EMAIL_FROM : L'adresse e-mail � partir de laquelle les e-mails seront envoy�s.
EMAIL_SMTP_SERVER : L'adresse du serveur SMTP pour l'envoi d'e-mails.
EMAIL_PORT : Le port utilis� par le serveur SMTP.
EMAIL_USERNAME : Le nom d'utilisateur pour l'authentification SMTP.
EMAIL_PASSWORD : Le mot de passe associ� au nom d'utilisateur pour l'authentification SMTP.

## Installation et Utilisation

Pour installer et utiliser l'application, suivez ces �tapes :

1. Clonez ce d�p�t sur votre machine locale.
2. Configurez les variables d'environnement comme d�crit ci-dessus en cr�ant un fichier `.env` � la racine du projet.
3. Appliquer les migrations
4. D�marrez l'application en ex�cutant `dotnet run`.
5. L'application sera accessible � l'adresse sp�cifi�e dans la documentation.

Note : Les d�pendances n�cessaires � l'application sont d�j� incluses dans le projet via NuGet et ne n�cessitent pas d'installation suppl�mentaire.