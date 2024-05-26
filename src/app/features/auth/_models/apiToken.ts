/**
 * La classe `apiToken` représente un objet utilisé dans l'application pour structurer les données relatives aux jetons d'accès et de rafraîchissement.
 * Ces jetons sont couramment utilisés dans les systèmes d'authentification pour assurer la sécurité et la gestion des sessions.
 */
export class apiToken {
  /**
   * Propriété représentant le jeton d'accès (Access Token) associé à l'objet `ApiToken`.
   * @type {string}
   */
  accessToken!: string;

  /**
   * Propriété représentant le jeton de rafraîchissement (Refresh Token) associé à l'objet `ApiToken`.
   * @type {string}
   */
  refreshToken!: string;
}
