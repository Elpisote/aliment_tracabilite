# aliment_tracabilite

## Introduction
Ce projet est une application développée en dotnet 6 et angular 17, conçue pour gérer la traçabilité des produits alimentaires dans un restaurant.

## Fonctionnalités

### Administrateur (Admin)
En tant qu'administrateur, vous pouvez :
- Accéder aux catégories de produits (ajout, modification, suppression).
- Accéder aux produits (ajout, modification, suppression).
- Consulter l'historique des stocks.
- Gérer les stocks (produits sortis pour utilisation).
- Consulter la liste des utilisateurs et modifier leur rôle.

### Utilisateur (User)
En tant qu'utilisateur, vous pouvez :
- Accéder aux stocks et retirer un ou plusieurs produits (par exemple, une cuisse de poulet).
- Imprimer une étiquette contenant les informations nécessaires (numéro du produit, nom, date d'ouverture et date de péremption).

### Liste des produits en stock
- Les produits dont la date de péremption est atteinte sont mis en évidence en rouge.
- Les utilisateurs (administrateurs ou utilisateurs) peuvent modifier l'état des produits comme suit :
    - En cours
    - Consommé
    - Erreur de saisie
    - Périmé
Seuls les produits en cours sont visibles dans l'onglet "Stocks". Les autres états sont disponibles dans l'historique.

## Gestion des Utilisateurs
Chaque rôle permet à l'utilisateur d'accéder à ses informations et de les modifier (nom, prénom, pseudo, mot de passe).

## Utilisation du Projet
Pour démarrer le projet, suivez les instructions dans le README de chaque branche.

### Utilisateur par Défaut
Le projet est livré avec un utilisateur déjà enregistré pour faciliter le démarrage :
- **Email**: admin@gmail.com
- **Mot de passe**: Admin123!
- **Pseudo**: Administrator
- **Rôle**: Admin
Pour accéder aux fonctionnalités administratives et explorer pleinement le projet, vous pouvez utiliser cet utilisateur par défaut.

## Structures des branches
Pour une meilleure organisation et une collaboration efficace, le projet est divisé en plusieurs branches spécifiques :

### Branches principales
- **backend** : contient tout le code relatif à l'API et à la logique serveur développés en dotnet 6.
- **frontend** : contient le code de l'interface utilisateur développé en Angular 17.
- **test** : utilisée pour l'écriture et l'exécution des tests (unitaires, intégration, etc.) du backend

### Démarrer avec les branches
1. **Backend**
-  Naviguez vers la branche backend : `git checkout backend`
- Suivez les instructions spécifiques dans le README de cette branche pour configurer et démarrer le serveur.

2. **Frontend**
-  Naviguez vers la branche frontnd : `git checkout frontend`
- Suivez les instructions spécifiques dans le README de cette branche pour configurer et démarrer l'application cliente.

3. **Test**
- Naviguez vers la branche test : `git checkout test`
- Suivez les instructions spécifiques dans le README de cette branche pour exécuter les tests et vérifier la fonctionnalité du projet.

## Structure du Répertoire Test
Pour une organisation claire, la branche test est divisée en deux dossiers principaux :

### Dossier de Test
- unit : contient les tests unitaires pour le backend
- integration : contient les tests d'intégration pour le backend 
