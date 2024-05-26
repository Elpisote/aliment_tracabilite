/**
 * Énumération représentant les types de noms de pages associés aux entités dans le système.
 * Chaque valeur de l'énumération correspond à un type d'entité spécifique et à son nom de page associé.
 */
export enum namePageType {
  /**
   * Type d'entité : Catégorie. Nom de la page associé : "CATEGORIES".
   */
  Category = 'CATEGORIES',

  /**
   * Type d'entité : Produit. Nom de la page associé : "PRODUITS".
   */
  Product = 'PRODUITS',

  /**
   * Type d'entité : Utilisateur. Nom de la page associé : "UTILISATEURS".
   */
  User = 'UTILISATEURS',

  /**
   * Type d'entité : Stock. Nom de la page associé : "STOCKS".
   */
  Stock = 'STOCKS',

  /**
   * Type d'entité : Historique. Nom de la page associé : "HISTORIQUES".
   */
  Historical = 'HISTORIQUES'
}
