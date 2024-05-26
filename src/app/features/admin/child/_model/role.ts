/**
 * La classe `Role` représente un rôle que l'utilisateur peut avoir dans l'application.
 * Elle stocke des informations sur un rôle, telles que son identifiant unique et son nom.
 */
export class Role {
  /**
   * Identifiant unique du rôle.
   * @type {number}
   */
  id!: string;

  /**
   * Nom du rôle.
   * @type {string}
   */
  name!: string;
}
