import { Action, createReducer, on } from "@ngrx/store";
import { LoginAction, SendMailAction, logout, updateCurrentUser } from "./auth-action";
import { AuthState, initialState } from "./auth-state";
import { CurrentUser } from "../_models/currentUser";

/**
 * Réducteur pour gérer l'état de l'authentification.
 * Ce réducteur traite les actions liées à l'authentification telles que la connexion réussie, la déconnexion et l'envoi d'e-mail.
 * Il met à jour l'état en conséquence, tel que l'utilisateur connecté et l'état d'envoi d'e-mail.
 */
const _authReducer = createReducer(
  initialState,
  on(LoginAction.login_success, (state, action) => {
    return {
      ...state,
      currentUser: action.currentUser    
    }
  }),  
  on(logout, (state) => {
    return {
      ...state,
      currentUser: null,
    };
  }),
  on(SendMailAction.send_mail_success, (state, action) => {
    return {
      ...state,
      sendMail: action.sendMail
    }
  }),
  on(updateCurrentUser, (state, action) => ({
    ...state,
    currentUser: action.currentUser
  }))
)

/**
 * Fonction réductrice pour l'état de l'authentification.
 * Cette fonction prend en charge la mise à jour de l'état en fonction des actions reçues.
 */
export function AuthReducer(state: AuthState | undefined, action: Action) {
  return _authReducer(state, action);
}
