import { CurrentUser } from "../_models/currentUser";

/**
 * Interface représentant l'état de l'authentification dans l'application.
 * Cet état contient les informations sur l'utilisateur actuellement connecté et l'état d'envoi d'e-mail.
 */
export interface AuthState {
  currentUser: CurrentUser | null; // L'utilisateur actuellement connecté, null s'il n'y a pas d'utilisateur connecté
  sendMail?: boolean; // Indique si un e-mail a été envoyé avec succès
}

/**
 * État initial de l'authentification.
 * Lorsque l'application démarre, aucun utilisateur n'est connecté et aucun e-mail n'a été envoyé.
 */
export const initialState: AuthState = {
  currentUser: null,
};
