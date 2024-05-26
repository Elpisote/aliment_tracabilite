import { CreatePassword } from "./createPassword";
import { Info } from "./info";

/**
 * L'interface `Register` est utilisée pour représenter les informations nécessaires à l'inscription d'un nouvel utilisateur.
 * Elle combine les données d'information utilisateur (Info) et de création de mot de passe (CreatePassword).
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité d'enregistrement de compte.
 */
export interface Register {
  /**
   * Informations sur l'utilisateur à enregistrer.
   * @type {Info}
   */
  info: Info;

  /**
   * Mot de passe à créer pour l'utilisateur lors de l'enregistrement.
   * @type {CreatePassword}
   */
  password: CreatePassword;
}
