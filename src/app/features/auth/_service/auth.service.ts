import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Store } from "@ngrx/store";
import { BehaviorSubject, Observable, map } from "rxjs";
import { URLBACKEND } from "../../../core/_utils/URL_BACKEND_constant";
import { LoadingService } from "../../../shared/_service/loading.service";
import { AuthResponse } from "../../auth/_models/authResponse";
import { apiToken } from "../_models/apiToken";
import { CurrentUser } from "../_models/currentUser";
import { ForgotPassword } from "../_models/forgotPassword";
import { Login } from "../_models/login";
import { Register } from "../_models/register";
import { ResetPassword } from "../_models/resetPassword";
import { UpdatePassword } from "../_models/updatePassword";
import { isAuthenticated, role, token } from "../_state/auth-selector";


@Injectable({ providedIn: "root" })

/**
* Le service `AuthService` gère les interactions avec le backend (Spring) pour l'authentification et la gestion des utilisateurs.
* Il comprend des méthodes pour se connecter, s'enregistrer, changer de mot de passe, réinitialiser le mot de passe, 
* envoyer un mail pour la récupération de mot de passe, obtenir les rôles de l'utilisateur connecté, obtenir le nom de l'utilisateur,
* obtenir le rôle de l'utilisateur connecté, rafraîchir le token et obtenir des messages d'erreur de validation.
*/
export class AuthService {
  isLoggedIn: boolean = false; // Indique si l'utilisateur est connecté
  role!: string; // Rôle de l'utilisateur
  token!: string; // Jeton d'authentification

  //injection des dépendances
  httpClient = inject(HttpClient)
  router = inject(Router)
  store = inject(Store)
  loadingService = inject(LoadingService)


  private currentUserSubject: BehaviorSubject<CurrentUser | null>;

  constructor() {
    this.currentUserSubject = new BehaviorSubject<CurrentUser | null>(null);
  }

  /**
  * Méthode pour se connecter via le backend.
  * @param login - Les informations de connexion de l'utilisateur.
  * @returns Observable de type AuthResponse.
  */
  login(login: Login): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(URLBACKEND.AUTH_LOGIN, login);   
  }

  /**
   * Méthode pour s'enregistrer.
   * @param register - Les informations d'inscription de l'utilisateur.
   * @returns Observable de type AuthResponse.
   */
  register(register: Register): Observable<AuthResponse> {
    return this.httpClient.post<AuthResponse>(URLBACKEND.AUTH_REGISTER, register);
  }

  /**
   * Méthode pour réinitialiser le mot de passe.
   * @param resetPassword - Les informations nécessaires pour réinitialiser le mot de passe.
   * @returns L'Observable résultant de la requête HTTP.
   */
  resetPassword(resetPassword: ResetPassword) {
    return this.httpClient.post<ResetPassword>(URLBACKEND.AUTH_RESET_PASSWORD, resetPassword)
  }

  /**
  * Méthode pour changer de mot de passe.
  * @param updatePassword - Les informations nécessaires pour changer le mot de passe.
  * @returns L'Observable résultant de la requête HTTP.
  */
  changePassword(updatePassword: UpdatePassword) {
    return this.httpClient.post<UpdatePassword>(URLBACKEND.AUTH_CHANGE_PASSWORD, updatePassword)
  }

  /**
  * Méthode permettant de rafraîchir le token.
  * @param apiT - Les informations nécessaires pour rafraîchir le token.
  * @returns Observable de type AuthResponse.
  */
  newToken(apiT: apiToken): Observable<AuthResponse>{
    return this.httpClient.post<AuthResponse>(URLBACKEND.TOKEN_REFRESH, apiT)
  }

  /**
   * Méthode pour envoyer un e-mail lorsqu'un mot de passe est oublié.
   * @param forgotPassword - Les informations nécessaires pour envoyer l'e-mail.
   * @returns L'Observable résultant de la requête HTTP.
   */
  sendMail(forgotPassword: ForgotPassword){
    return this.httpClient.post<AuthResponse>(URLBACKEND.AUTH_FORGOT_PASSWORD, forgotPassword)
  }

  /**
   * Déconnecte l'utilisateur en effaçant les informations de l'utilisateur stockées localement.
   */
  logout() {
    localStorage.clear();
  } 

  /**
  * Formate les données de réponse d'authentification en un objet `CurrentUser`.
  * @param data Les données de réponse d'authentification.
  * @returns Une instance de `CurrentUser` avec les informations d'utilisateur.
  */
  formatUser(data: AuthResponse) {
    if (!data || !data.accessToken) {
      return null; // Si data ou accessToken est null, retourne null
    }

    const userInfo = this.getUserInfoFromToken(data.accessToken);

    // Si userInfo est null ou les propriétés nécessaires sont manquantes, retourne null
    if (!userInfo || !userInfo.username || !userInfo.email || !userInfo.role) {
      return null;
    }

    // Sinon, crée et retourne une instance de CurrentUser
    return new CurrentUser(
      userInfo.username,
      userInfo.email,
      userInfo.role,
      data.accessToken,
      data.refreshToken
    );
  }

  /**
   * Enregistre les informations de l'utilisateur dans le stockage local du navigateur.
   * @param user Les informations de l'utilisateur à enregistrer.
   */
  setUserInLocalStorage(user: CurrentUser) {
    Object.entries(user).forEach(([key, value]) => {
      localStorage.setItem(key, value);
    });
  }

  /**
  * Récupère les informations de l'utilisateur depuis le stockage local du navigateur.
  * @returns Une instance de `CurrentUser` si les informations sont disponibles, sinon null.
  */
  getUserFromLocalStorage() {
    const username = localStorage.getItem('username');
    const email = localStorage.getItem('email');
    const role = localStorage.getItem('role');
    const token = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refreshToken');

    if (username && email && role && refreshToken && token) {   
      return new CurrentUser(username, email, role, token, refreshToken);
    } else {         
      return null;
    }
  }

  /**
   * Vérifie si l'utilisateur est actuellement authentifié.
   * @returns true si l'utilisateur est authentifié, sinon false.
   */
  getIsAuthenticated(): Observable<boolean> {
    return this.store.select(isAuthenticated).pipe(
      map(value => !!value) // Transforme la valeur en booléen (true si la valeur est définie, false sinon)
    );
  }

  /**
   * Récupère le rôle de l'utilisateur actuellement connecté.
   * @returns Le rôle de l'utilisateur.
   */
  getCurrentRole() {
    this.store.select(role).subscribe((value: string) => {
      this.role = value;
    });
    return this.role;
  }

  /**
   * Récupère le jeton d'authentification de l'utilisateur actuellement connecté.
   * @returns Le jeton d'authentification.
   */
  getToken() {
    this.store.select(token).subscribe((value: string) => {
      this.token = value;
    });
    return this.token;
  }

  /**
    * Met à jour les informations de l'utilisateur actuellement connecté.
    * @param user Les nouvelles informations de l'utilisateur.
    */
  updateUser(user: CurrentUser): void {
    this.currentUserSubject.next(user);    
  }

  /**
   * Parse les informations de l'utilisateur à partir du jeton JWT.
   * @param token Le jeton JWT.
   * @returns Les informations de l'utilisateur extraites du jeton.
   */
  private getUserInfoFromToken(token: string): {
    email: string, role: string, username: string
  } {
    let helper = new JwtHelperService();
    const decodedToken = helper.decodeToken(token);
    const email = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];
    const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const username = decodedToken['Username'];

    return { email, role, username };
  }
}
