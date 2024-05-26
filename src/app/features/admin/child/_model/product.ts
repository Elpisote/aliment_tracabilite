import { Category } from "./category";

/**
 * La classe `Product` représente un produit alimentaire dans l'application.
 * Elle stocke des informations sur un produit, telles que son identifiant, son nom, sa description,
 * la durée de conservation, la catégorie à laquelle il appartient, l'identifiant de la catégorie,
 * et le nombre de produits en stock.
 */
export class Product {
  /**
   * Identifiant unique du produit.
   * @type {number}
   */
  id!: number;

  /**
   * Nom du produit.
   * @type {string}
   */
  name!: string;

  /**
   * Description du produit.
   * @type {string}
   */
  description!: string;

  /**
   * Durée de conservation du produit en jours.
   * @type {number}
   */
  durationConservation!: number;

  /**
   * Objet représentant la catégorie à laquelle le produit appartient.
   * @type {Category}
   */
  category!: Category;

  /**
   * Identifiant unique de la catégorie à laquelle le produit appartient.
   * @type {number}
   */
  categoryId!: number;

  /**
   * Nombre de produits en stock pour ce produit.
   * @type {number}
   */
  nbProductStock!: number;
}
