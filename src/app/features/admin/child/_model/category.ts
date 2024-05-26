/**
 * La classe `Category` représente une catégorie d'aliment dans l'application.
 * Elle est utilisée pour structurer les données relatives à une catégorie.
 */
export class Category {
  /**
   * Identifiant unique de la catégorie.
   * @type {number}
   */
  id!: number;

  /**
   * Nom de la catégorie.
   * @type {string}
   */
  name!: string;

  /**
   * Description de la catégorie.
   * @type {string}
   */
  description!: string;

  /**
   * Nombre de produits associés à cette catégorie.
   * @type {number}
   */
  nbProduct!: number;
}
