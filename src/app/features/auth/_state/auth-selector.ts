import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AuthState } from "./auth-state";

/**
 * Nom de l'état de l'authentification dans le store.
 */
export const AUTH_STATE_NAME = 'auth';

/**
 * Sélecteur pour obtenir l'état de l'authentification à partir du store.
 */
const getAuthState = createFeatureSelector<AuthState>(AUTH_STATE_NAME);

/**
 * Sélecteur pour vérifier si un utilisateur est authentifié.
 * Renvoie vrai si l'utilisateur a un token valide, sinon faux.
 */
export const isAuthenticated = createSelector(getAuthState, state => state.currentUser?.getToken ? true : false);

/**
 * Sélecteur pour obtenir le nom d'utilisateur.
 * Renvoie le nom d'utilisateur s'il est disponible, sinon 'invité'.
 */
export const username = createSelector(getAuthState, state => state.currentUser?.getUsername() || 'invité');

/**
 * Sélecteur pour obtenir le rôle de l'utilisateur.
 * Renvoie le rôle de l'utilisateur s'il est disponible, sinon une chaîne vide.
 */
export const role = createSelector(getAuthState, state => state.currentUser?.getRole() || '');

/**
 * Sélecteur pour vérifier si un e-mail a été envoyé avec succès.
 * Renvoie vrai si l'e-mail a été envoyé avec succès, sinon faux.
 */
export const isSendMail = createSelector(getAuthState, state => state.sendMail ?? false);

/**
 * Sélecteur pour obtenir le token de l'utilisateur.
 * Renvoie le token de l'utilisateur s'il est disponible, sinon une chaîne vide.
 */
export const token = createSelector(getAuthState, state => state.currentUser?.getToken() || '');




