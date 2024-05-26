/**
 * Représente les informations de l'utilisateur actuellement connecté.
 */
export class CurrentUser {
  private username!: string; // Nom d'utilisateur
  private email!: string; // Adresse email
  private role!: string; // Rôle de l'utilisateur
  private token: string; // Jeton d'authentification
  private refreshToken: string; // Jeton de rafraîchissement

  /**
   * Crée une nouvelle instance de CurrentUser.
   * @param username Le nom d'utilisateur.
   * @param email L'adresse email.
   * @param role Le rôle de l'utilisateur.
   * @param token Le jeton d'authentification.
   * @param refreshToken Le jeton de rafraîchissement.
   */
  constructor(username: string, email: string, role: string, token: string, refreshToken: string) {
    this.username = username;
    this.email = email;
    this.role = role;
    this.token = token;
    this.refreshToken = refreshToken;
  }

  /**
   * Récupère le nom d'utilisateur.
   * @returns Le nom d'utilisateur.
   */
  getUsername() {
    return this.username;
  }

  /**
   * Récupère le rôle de l'utilisateur.
   * @returns Le rôle de l'utilisateur.
   */
  getRole() {
    return this.role;
  }

  /**
   * Récupère le jeton d'authentification.
   * @returns Le jeton d'authentification.
   */
  getToken() {
    return this.token;
  }

  /**
  * Récupère le mail.
  * @returns Le mail.
  */
  getEmail() {
    return this.email;
  }

  getRefreshToken() {
    return this.refreshToken
  }
}

