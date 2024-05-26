import { CreatePassword } from "./createPassword";

/**
 * L'interface `ResetPassword` est utilisée pour représenter les informations nécessaires à la réinitialisation d'un mot de passe.
 * Elle inclut les données du nouveau mot de passe (CreatePassword), l'adresse e-mail associée au compte, et le jeton de réinitialisation.
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité de réinitialisation de mot de passe.
 */
export interface ResetPassword {
  /**
   * Nouveau mot de passe à définir lors de la réinitialisation.
   * @type {CreatePassword}
   */
  password: CreatePassword;

  /**
   * Adresse e-mail associée au compte pour la réinitialisation du mot de passe.
   * @type {string}
   */
  email: string;

  /**
   * Jeton de réinitialisation du mot de passe.
   * @type {string}
   */
  token: string;
}
