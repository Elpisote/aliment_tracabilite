/**
 * L'interface `ForgotPassword` est utilisée pour représenter les données nécessaires à la perte d'un mot de passe.
 * Elle est généralement utilisée dans le cadre d'une fonctionnalité de récupération de mot de passe.
 */
export interface ForgotPassword {
  /**
   * Adresse e-mail du destinataire pour la récupération du mot de passe.
   * @type {string}
   */
  email: string
}
