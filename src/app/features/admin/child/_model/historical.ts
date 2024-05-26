import { Stock } from "../../../../shared/module/stock/_model/stock";

/**
 * La classe `Historical` représente un enregistrement historique d'une action effectuée dans l'application.
 * Elle stocke des informations sur une opération passée, comme la date, l'action effectuée, et les détails du stock associé.
 */
export class Historical {
  /**
   * Identifiant unique de l'enregistrement historique.
   * @type {number}
   */
  id!: number;

  /**
   * Date à laquelle l'action historique a été effectuée.
   * @type {Date}
   */
  controleDate!: Date;

  /**
   * Action effectuée dans l'enregistrement historique, par exemple, "Ajout", "Modification", "Suppression", etc.
   * @type {string}
   */
  action!: string;

  /**
   * Objet représentant le stock associé à cet enregistrement historique.
   * @type {Stock}
   */
  stock!: Stock;

  /**
   * Identifiant unique du stock associé à cet enregistrement historique.
   * @type {number}
   */
  stockId!: number;
}
