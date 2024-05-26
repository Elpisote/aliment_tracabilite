/**
 * L'interface `CreatePassword` est utilisée pour représenter les données nécessaires à la création d'un mot de passe.
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité de création de compte ou de changement de mot de passe.
 */
export interface CreatePassword {
  /**
   * Propriété représentant le mot de passe à créer.
   * @type {string}
   */
  password: string;
}
