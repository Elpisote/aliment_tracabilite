/**
 * L'interface `AuthResponse` est utilisée pour représenter la réponse lors de la demande du token.
 * Elle peut être utilisée pour transmettre des informations sur le succès ou l'échec d'une opération,
 * ainsi que des données associées telles qu'un message, un jeton d'accès et un jeton de rafraîchissement.
 */
export interface AuthResponse {
  /**
   * Indique si l'opération a réussi. Peut être `true` pour succès, `false` pour échec ou `undefined`.
   * @type {boolean}
   */
  isSucceed: boolean;
  /**
   * Message associé à la réponse. Il peut contenir des informations détaillées sur le résultat de l'opération.
   * @type {string}
   */
  message: string;

  /**
   * Jeton d'accès associé à la réponse. Utilisé dans le contexte de l'authentification pour les accès sécurisés.
   * @type {string}
   */
  accessToken: string;

  /**
   * Jeton de rafraîchissement associé à la réponse. Utilisé dans le contexte de l'authentification pour obtenir de nouveaux jetons d'accès.
   * @type {string}
   */
  refreshToken: string;
}
