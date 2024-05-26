import { Product } from "../../../../features/admin/child/_model/product";


/**
 * Classe représentant un objet de stock dans le système.
 */
export class Stock {
  /**
   * Identifiant unique du stock.
   */
  id!: number;

  /**
   * Statut du stock.
   */
  statuts!: number;

  /**
   * Nom de l'utilisateur qui a créé le stock.
   */
  userCreation!: string;

  /**
   * Nom de l'utilisateur qui a modifié le stock.
   */
  userModification!: string;

  /**
   * Date d'ouverture du stock.
   */
  openingDate!: Date;

  /**
   * Identifiant du produit associé au stock.
   */
  productId!: number;

  /**
   * Objet représentant le produit associé au stock.
   */
  product!: Product;

  /**
   * Temps restant avant la fin du stock.
   */
  countdown!: string;

  /**
   * Produit vérifié associé au stock.
   */
  verifiedProduct!: Product;
}


