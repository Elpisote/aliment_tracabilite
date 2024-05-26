/**
 * L'interface `Info` est utilisée pour représenter les données nécessaires à la création d'un utilisateur.
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité de création de compte ou de changement des données de l'utilisateur.
 */

export interface Info {
  /**
   * Prénom de l'utilisateur.
   * @type {string}
   */
  firstName: string;

  /**
   * Nom de famille de l'utilisateur.
   * @type {string}
   */
  lastName: string;

  /**
   * Pseudo (nom d'utilisateur) de l'utilisateur.
   * @type {string}
   */
  username: string;

  /**
   * Adresse e-mail de l'utilisateur.
   * @type {string}
   */
  email: string;
}
