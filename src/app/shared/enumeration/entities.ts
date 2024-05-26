/**
 * Énumération représentant les types d'entités disponibles dans le système.
 * Chaque valeur de l'énumération correspond à un type d'entité spécifique.
 */
export enum entitiesType {
  /**
   * Type d'entité : Catégorie.
   */
  Category = 'Category',

  /**
   * Type d'entité : Produit.
   */
  Product = 'Product',

  /**
   * Type d'entité : Utilisateur.
   */
  User = 'User',

  /**
   * Type d'entité : Stock.
   */
  Stock = 'Stock',

  /**
   * Type d'entité : Historique.
   */
  Historical = 'Historical'
}
