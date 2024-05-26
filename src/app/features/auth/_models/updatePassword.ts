import { CreatePassword } from "./createPassword";

/**
 * L'interface `UpdatePassword` est utilisée pour représenter les informations nécessaires à la mise à jour du mot de passe d'un utilisateur.
 * Elle inclut les données du nouveau mot de passe (CreatePassword) ainsi que le nom d'utilisateur associé.
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité permettant à un utilisateur de modifier son mot de passe.
 */
export interface UpdatePassword {
  /**
   * Nouveau mot de passe à définir lors de la mise à jour.
   * @type {CreatePassword}
   */
  password: CreatePassword;

  /**
   * Nom d'utilisateur associé à la mise à jour du mot de passe.
   * @type {string}
   */
  userName: string;
}
