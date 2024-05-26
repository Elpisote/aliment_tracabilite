/**
 * L'interface `Login` est utilisée pour représenter les informations nécessaires à l'authentification d'un utilisateur.
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité de connexion.
 */
export interface Login {
  /**
   * Adresse e-mail de l'utilisateur pour l'authentification.
   * @type {string}
   */
  email: string;

  /**
   * Mot de passe de l'utilisateur pour l'authentification.
   * @type {string}
   */
  password: string;
}
